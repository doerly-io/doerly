using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

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
                    Value = fv.Value
                }).ToList()
            };

            return OperationResult.Success(response);
        }
    }
}
