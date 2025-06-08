using Doerly.Domain.Models;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServicesByUserIdHandler : BaseCatalogHandler
    {
        public GetServicesByUserIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<List<GetServiceResponse>>> HandleAsync(int userId)
        {
            var services = await DbContext.Services
                .AsNoTracking()
                .Where(s => !s.IsDeleted && s.UserId == userId)
                .ToListAsync();

            var allCategoryIds = services.Select(s => s.CategoryId).Distinct();
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .Where(c => allCategoryIds.Contains(c.Id) || c.ParentId != null)
                .ToListAsync();

            var categoryDict = allCategories.ToDictionary(c => c.Id);

            var result = services.Select(s => new GetServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                UserId = s.UserId,
                IsEnabled = s.IsEnabled,
                IsDeleted = s.IsDeleted,
                CategoryId = s.CategoryId,
                CategoryName = categoryDict.TryGetValue(s.CategoryId.Value, out var category) ? category.Name : "",
                CategoryPath = GetCategoryPath(categoryDict, s.CategoryId)
            }).ToList();

            return HandlerResult.Success(result);
        }

        private List<string> GetCategoryPath(Dictionary<int, CategoryEntity> categoryDict, int? categoryId)
        {
            var path = new List<string>();
            while (categoryId != null && categoryDict.TryGetValue(categoryId.Value, out var category))
            {
                path.Insert(0, category.Name);
                categoryId = category.ParentId;
            }
            return path;
        }
    }
}
