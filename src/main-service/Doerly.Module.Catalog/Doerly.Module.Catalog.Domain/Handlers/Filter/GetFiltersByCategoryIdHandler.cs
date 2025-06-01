using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Filter;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class GetFiltersByCategoryIdHandler : BaseCatalogHandler
    {
        public GetFiltersByCategoryIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<List<GetFilterResponse>>> HandleAsync(int categoryId)
        {
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .ToDictionaryAsync(c => c.Id);

            if (!allCategories.ContainsKey(categoryId))
            {
                return HandlerResult.Failure<List<GetFilterResponse>>(Resources.Get("CategoryNotFound"));
            }

            var categoryIds = new List<int>();
            int? currentId = categoryId;

            while (currentId.HasValue)
            {
                categoryIds.Add(currentId.Value);
                var category = allCategories.GetValueOrDefault(currentId.Value);
                currentId = category?.ParentId;
            }

            var filters = await DbContext.Filters
                .AsNoTracking()
                .Where(f => categoryIds.Contains(f.CategoryId))
                .ToListAsync();

            var dtos = filters.Select(f => new GetFilterResponse
            {
                Id = f.Id,
                Name = f.Name,
                Type = (Enums.EFilterType)f.Type,
                CategoryId = f.CategoryId
            }).ToList();

            return HandlerResult.Success(dtos);
        }
    }
}
