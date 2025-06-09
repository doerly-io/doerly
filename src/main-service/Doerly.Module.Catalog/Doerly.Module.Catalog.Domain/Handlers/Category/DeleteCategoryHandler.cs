using Doerly.Domain.Models;
using Doerly.Module.Catalog.DataAccess;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class DeleteCategoryHandler : BaseCatalogHandler
    {
        public DeleteCategoryHandler(CatalogDbContext dbContext) : base(dbContext) { }

        public async Task<OperationResult> HandleAsync(int id)
        {
            var category = await DbContext.Categories.FindAsync(id);
            category.IsDeleted = true;

            await DbContext.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
