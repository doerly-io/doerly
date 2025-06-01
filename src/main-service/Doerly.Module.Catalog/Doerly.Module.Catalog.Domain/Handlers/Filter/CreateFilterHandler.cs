using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Category;
using Doerly.Module.Catalog.DataAccess;
using FilterEntity = Doerly.Module.Catalog.DataAccess.Models.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Filter;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class CreateFilterHandler : BaseCatalogHandler
    {
        public CreateFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<int>> HandleAsync(CreateFilterRequest request)
        {
            var category = await DbContext.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return HandlerResult.Failure<int>(Resources.Get("CategoryNotFound"));

            var filter = new FilterEntity
            {
                Name = request.Name,
                Type = ((int)request.Type),
                CategoryId = request.CategoryId
            };

            DbContext.Filters.Add(filter);
            await DbContext.SaveChangesAsync();

            return HandlerResult.Success(filter.Id);
        }
    }
}
