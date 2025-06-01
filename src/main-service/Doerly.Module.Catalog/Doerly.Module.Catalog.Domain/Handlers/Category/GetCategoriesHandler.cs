using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Category;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Service;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class GetCategoriesHandler : BaseCatalogHandler
    {
        public GetCategoriesHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<List<GetCategoryResponse>>> HandleAsync()
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

            return HandlerResult.Success(rootCategories);
        }
    }
}
