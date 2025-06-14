using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServiceByIdHandler : BaseCatalogHandler
    {
        private readonly IProfileModuleProxy _profileModuleProxy;
        public GetServiceByIdHandler(CatalogDbContext dbContext, IProfileModuleProxy profileModuleProxy) : base(dbContext)
        {
            _profileModuleProxy = profileModuleProxy;
        }

        public async Task<OperationResult<GetServiceResponse>> HandleAsync(int id)
        {
            var service = await DbContext.Services
                .Include(s => s.FilterValues)
                    .ThenInclude(fv => fv.Filter)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return OperationResult.Failure<GetServiceResponse>(Resources.Get("ServiceNotFound"));

            var userProfile = await _profileModuleProxy.GetProfileAsync(service.UserId);

            var response = new GetServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                CategoryId = service.Category.Id,
                CategoryName = service.Category.Name,
                UserId = service.UserId,
                User = userProfile.Value,
                Price = service.Price,
                IsDeleted = service.IsDeleted,
                IsEnabled = service.IsEnabled,
                FilterValues = service.FilterValues.Select(fv => new FilterValueResponse
                {
                    FilterId = fv.FilterId,
                    FilterName = fv.Filter.Name,
                    FilterType = fv.Filter.Type,
                    Value = fv.Value
                }).ToList()
            };

            var otherServices = await DbContext.Services
                .Include(s => s.Category)
                .Include(s => s.FilterValues)
                    .ThenInclude(fv => fv.Filter)
                .Where(s => s.CategoryId == service.Category.Id && s.Id != service.Id && s.IsEnabled && !s.IsDeleted)
                .ToListAsync();

            var enrichedServices = new List<(ServiceEntity Service, ProfileDto Profile, float? CompetenceRating)>();

            foreach (var s in otherServices)
            {
                var profileResult = await _profileModuleProxy.GetProfileAsync(s.UserId);
                if (!profileResult.IsSuccess || profileResult.Value == null)
                    continue;

                var profile = profileResult.Value;
                var competenceRating = profile.Competences
                    .FirstOrDefault(c => c.CategoryId == service.Category.Id)?.Rating;

                enrichedServices.Add((s, profile, competenceRating));
            }

            var topServices = enrichedServices
                .OrderByDescending(x => x.CompetenceRating ?? -1)
                .ThenByDescending(x => x.Profile.Rating ?? -1)
                .ThenBy(x => x.Service.Price)
                .Take(3)
                .Select(x => new GetServiceResponse
                {
                    Id = x.Service.Id,
                    Name = x.Service.Name,
                    Description = x.Service.Description,
                    CategoryId = x.Service.CategoryId,
                    CategoryName = x.Service.Category?.Name,
                    UserId = x.Service.UserId,
                    User = x.Profile,
                    Price = x.Service.Price,
                    IsEnabled = x.Service.IsEnabled,
                    IsDeleted = x.Service.IsDeleted,
                    FilterValues = x.Service.FilterValues.Select(fv => new FilterValueResponse
                    {
                        FilterId = fv.FilterId,
                        FilterName = fv.Filter.Name,
                        FilterType = fv.Filter.Type,
                        Value = fv.Value
                    }).ToList()
                })
                .ToList();

            response.RecommendedServices = topServices;

            return OperationResult.Success(response);
        }
    }
}
