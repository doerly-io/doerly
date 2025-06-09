using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class CreateServiceHandler : BaseCatalogHandler
    {
        public CreateServiceHandler(CatalogDbContext dbContext) : base(dbContext) 
        { 
        }

        public async Task<OperationResult<int>> HandleAsync(CreateServiceRequest request)
        {
            var categoryExists = await DbContext.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                return OperationResult.Failure<int>(Resources.Get("CategoryNotFound"));

            var service = new ServiceEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                IsEnabled = true,
            };

            var filterIds = await DbContext.Filters
                .Where(f => f.CategoryId == request.CategoryId)
                .Select(f => f.Id)
                .ToListAsync();

            foreach (var filterId in filterIds)
            {
                if (request.FilterValues.TryGetValue(filterId, out var value))
                {
                    service.FilterValues.Add(new ServiceFilterValue
                    {
                        FilterId = filterId,
                        Value = value
                    });
                }
            }

            DbContext.Services.Add(service);
            await DbContext.SaveChangesAsync();

            return OperationResult.Success(service.Id);
        }
    }
}
