using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class GetFiltersByCategoryIdHandler : BaseCatalogHandler
    {
        public GetFiltersByCategoryIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<List<GetFilterResponse>>> HandleAsync(int categoryId)
        {
            var parentCategoryIds = await GetAllParentCategoryIds(categoryId);
            
            var query = DbContext.Filters
                    .Include(s => s.FilterValues)
                .Where(f => !f.Category.IsDeleted && parentCategoryIds.Contains(f.CategoryId))
                .Select(f => new GetFilterResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    CategoryId = f.CategoryId,
                    Values = f.FilterValues.Select(fv => fv.Value).ToList()
                });

            var filters = await query.ToListAsync();

            return OperationResult.Success(filters);
        }

        private async Task<List<int>> GetAllParentCategoryIds(int categoryId)
        {
            var parentIds = new List<int>();
            var currentCategoryId = (int?)categoryId;

            while (currentCategoryId.HasValue)
            {
                parentIds.Add(currentCategoryId.Value);

                var id = currentCategoryId;
                var parentId = await DbContext.Categories
                    .Where(c => c.Id == id.Value)
                    .Select(c => c.ParentId)
                    .FirstOrDefaultAsync();

                currentCategoryId = parentId;
            }

            return parentIds;
        }
    }
}
