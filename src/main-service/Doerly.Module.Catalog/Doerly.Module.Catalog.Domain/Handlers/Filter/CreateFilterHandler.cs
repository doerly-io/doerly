using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using FilterEntity = Doerly.Module.Catalog.DataAccess.Models.Filter;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class CreateFilterHandler : BaseCatalogHandler
    {
        public CreateFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<int>> HandleAsync(CreateFilterRequest request)
        {
            var categoryExists = await DbContext.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);

            if (!categoryExists)
                return OperationResult.Failure<int>(Resources.Get("CategoryNotFound"));

            var filter = new FilterEntity
            {
                Name = request.Name,
                Type = request.Type,
                CategoryId = request.CategoryId
            };

            DbContext.Filters.Add(filter);
            await DbContext.SaveChangesAsync();

            return OperationResult.Success(filter.Id);
        }
    }
}
