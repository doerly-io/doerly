using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
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
            var allChildCategoryIds = await GetAllChildCategoryIds(categoryId);

            var query = DbContext.Filters
                .Include(f => f.FilterValues)
                    .ThenInclude(fv => fv.Service)
                .Where(f => !f.Category.IsDeleted && allChildCategoryIds.Contains(f.CategoryId))
                .Select(f => new GetFilterResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    CategoryId = f.CategoryId,
                    Values = f.FilterValues
                        .Where(fv =>
                            fv.Service != null &&
                            fv.Service.IsEnabled &&
                            !fv.Service.IsDeleted &&
                            !string.IsNullOrWhiteSpace(fv.Value))
                        .Select(fv => fv.Value)
                        .Distinct()
                        .ToList()
                });

            var filters = await query.ToListAsync();

            return OperationResult.Success(filters);
        }

        private async Task<List<int>> GetAllChildCategoryIds(int rootCategoryId)
        {
            var allCategories = await DbContext.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new { c.Id, c.ParentId })
                .ToListAsync();

            var result = new List<int>();
            var stack = new Stack<int>();
            stack.Push(rootCategoryId);

            while (stack.Count > 0)
            {
                var currentId = stack.Pop();
                result.Add(currentId);

                var children = allCategories
                    .Where(c => c.ParentId == currentId)
                    .Select(c => c.Id);

                foreach (var childId in children)
                {
                    stack.Push(childId);
                }
            }

            return result;
        }
    }
}
