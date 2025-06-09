using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class UpdateCategoryHandler : BaseCatalogHandler
    {
        public UpdateCategoryHandler(CatalogDbContext dbContext) : base(dbContext) 
        { 
        }

        public async Task<OperationResult> HandleAsync(int id, UpdateCategoryRequest request)
        {
            if (request.ParentId == id)
                return OperationResult.Failure(Resources.Get("CategoryCannotBeOwnParent"));

            var category = await DbContext.Categories.FindAsync(id);
            if (category == null)
                return OperationResult.Failure(Resources.Get("CategoryNotFound"));

            if (request.ParentId.HasValue)
            {
                var parentExists = await DbContext.Categories.AnyAsync(c => c.Id == request.ParentId);
                if (!parentExists)
                    return OperationResult.Failure(Resources.Get("ParentCategoryNotFound"));
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.ParentId = request.ParentId;
            category.IsEnabled = request.IsEnabled;

            await DbContext.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}
