using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class DeleteFilterHandler : BaseCatalogHandler
    {
        public DeleteFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult> HandleAsync(int id)
        {
            var filter = await DbContext.Filters.FindAsync(id);
            if (filter == null)
                return HandlerResult.Failure(Resources.Get("FilterNotFound"));

            DbContext.Filters.Remove(filter);
            await DbContext.SaveChangesAsync();
            return HandlerResult.Success();
        }
    }
}
