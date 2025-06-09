using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class GetCategoriesHandler : BaseCatalogHandler
    {
        public GetCategoriesHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<List<GetCategoryResponse>>> HandleAsync()
        {
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .ToListAsync();

            var lookup = allCategories.ToLookup(c => c.ParentId);

            List<GetCategoryResponse> BuildTree(int? parentId)
            {
                return lookup[parentId]
                    .Select(c => new GetCategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        IsDeleted = c.IsDeleted,
                        IsEnabled = c.IsEnabled,
                        Children = BuildTree(c.Id)
                    })
                    .ToList();
            }

            var rootCategories = BuildTree(null);

            return OperationResult.Success(rootCategories);
        }
    }
}
