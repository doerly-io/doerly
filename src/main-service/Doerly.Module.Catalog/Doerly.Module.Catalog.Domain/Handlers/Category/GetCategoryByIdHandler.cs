using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class GetCategoryByIdHandler : BaseCatalogHandler
    {
        public GetCategoryByIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<GetCategoryResponse>> HandleAsync(int id)
        {
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .ToListAsync();

            var category = allCategories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return OperationResult.Failure<GetCategoryResponse>(Resources.Get("CategoryNotFound"));

            var lookup = allCategories.ToLookup(c => c.ParentId);

            GetCategoryResponse BuildTree(CategoryEntity category)
            {
                return new GetCategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsDeleted = category.IsDeleted,
                    IsEnabled = category.IsEnabled,
                    Children = lookup[category.Id].Select(BuildTree).ToList()
                };
            }

            var categoryDto = BuildTree(category);

            return OperationResult.Success(categoryDto);
        }
    }
}
