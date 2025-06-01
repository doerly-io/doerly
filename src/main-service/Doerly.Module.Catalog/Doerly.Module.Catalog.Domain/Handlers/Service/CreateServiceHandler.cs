using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Service;
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

        public async Task<HandlerResult<int>> HandleAsync(CreateServiceRequest request)
        {
            var categoryExists = await DbContext.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                return HandlerResult.Failure<int>(Resources.Get("CategoryNotFound"));

            var service = new ServiceEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                IsEnabled = true,
                IsDeleted = false
            };

            var filters = await DbContext.Filters
                .Where(f => f.CategoryId == request.CategoryId)
                .ToListAsync();

            foreach (var filter in filters)
            {
                if (request.FilterValues.TryGetValue(filter.Id, out var value))
                {
                    service.FilterValues.Add(new ServiceFilterValue
                    {
                        FilterId = filter.Id,
                        Value = value
                    });
                }
            }

            DbContext.Services.Add(service);
            await DbContext.SaveChangesAsync();

            return HandlerResult.Success(service.Id);
        }
    }
}
