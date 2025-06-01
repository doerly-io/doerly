using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Filter;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class UpdateFilterHandler : BaseCatalogHandler
    {
        public UpdateFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult> HandleAsync(UpdateFilterRequest request)
        {
            var filter = await DbContext.Filters.FindAsync(request.Id);
            if (filter == null)
                return HandlerResult.Failure(Resources.Get("FilterNotFound"));

            var categoryExists = await DbContext.Categories
                    .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                return HandlerResult.Failure(Resources.Get("CategoryNotFound"));

            filter.Name = request.Name;
            filter.Type = ((int)request.Type);
            filter.CategoryId = request.CategoryId;

            await DbContext.SaveChangesAsync();
            return HandlerResult.Success();
        }
    }
}
