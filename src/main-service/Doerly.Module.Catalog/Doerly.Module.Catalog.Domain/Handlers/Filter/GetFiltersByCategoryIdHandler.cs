using Doerly.Domain.Models;
using Doerly.Localization;
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

        public async Task<HandlerResult<List<GetFilterResponse>>> HandleAsync(int categoryId)
        {
            var query = DbContext.Filters
                .Where(f => !f.Category.IsDeleted &&
                            (f.CategoryId == categoryId ||
                             f.Category.ParentId == categoryId ||
                             f.Category.Parent.ParentId == categoryId))
                .Select(f => new GetFilterResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    CategoryId = f.CategoryId
                });

            var filters = await query.ToListAsync();

            if (!filters.Any())
                return HandlerResult.Failure<List<GetFilterResponse>>(Resources.Get("CategoryNotFound"));

            return HandlerResult.Success(filters);
        }
    }
}
