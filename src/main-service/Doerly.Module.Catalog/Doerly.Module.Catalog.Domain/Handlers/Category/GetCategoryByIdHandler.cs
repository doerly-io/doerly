using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Category;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class GetCategoryByIdHandler : BaseCatalogHandler
    {
        public GetCategoryByIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<GetCategoryResponse>> HandleAsync(int id)
        {
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .ToListAsync();

            var category = allCategories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return HandlerResult.Failure<GetCategoryResponse>(Resources.Get("CategoryNotFound"));

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

            return HandlerResult.Success(categoryDto);
        }
    }
}
