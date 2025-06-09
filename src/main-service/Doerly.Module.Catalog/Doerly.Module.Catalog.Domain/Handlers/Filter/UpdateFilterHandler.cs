using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class UpdateFilterHandler : BaseCatalogHandler
    {
        public UpdateFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> HandleAsync(int id, UpdateFilterRequest request)
        {
            var filter = await DbContext.Filters.FindAsync(id);
            if (filter == null)
                return OperationResult.Failure(Resources.Get("FilterNotFound"));

            var categoryExists = await DbContext.Categories
                    .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                return OperationResult.Failure(Resources.Get("CategoryNotFound"));

            filter.Name = request.Name;
            filter.Type = request.Type;
            filter.CategoryId = request.CategoryId;

            await DbContext.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
