using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class UpdateServiceHandler : BaseCatalogHandler
    {
        public UpdateServiceHandler(CatalogDbContext dbContext) : base(dbContext) 
        { 
        }

        public async Task<OperationResult> HandleAsync(int id, UpdateServiceRequest request)
        {
            var service = await DbContext.Services
                .Include(s => s.FilterValues)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null || service.IsDeleted)
                return OperationResult.Failure(Resources.Get("ServiceNotFound"));

            var categoryExists = await DbContext.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                return OperationResult.Failure(Resources.Get("CategoryNotFound"));

            service.Name = request.Name;
            service.Description = request.Description;
            service.Price = request.Price;
            service.CategoryId = request.CategoryId;
            service.IsEnabled = request.IsEnabled;

            DbContext.ServiceFilterValues.RemoveRange(service.FilterValues);

            if (request.FilterValues != null)
            {
                foreach (var kvp in request.FilterValues)
                {
                    var filterId = kvp.Key;
                    var value = kvp.Value;

                    var filterExists = await DbContext.Filters
                        .AnyAsync(f => f.Id == filterId && f.CategoryId == request.CategoryId);

                    if (!filterExists)
                        continue;

                    service.FilterValues.Add(new ServiceFilterValue
                    {
                        FilterId = filterId,
                        Value = value,
                        Service = service
                    });
                }
            }

            await DbContext.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
