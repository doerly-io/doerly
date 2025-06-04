using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Filter
{
    public class DeleteFilterHandler : BaseCatalogHandler
    {
        public DeleteFilterHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult> HandleAsync(int id)
        {
            var deletedCount = await DbContext.Filters
                .Where(f => f.Id == id)
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
                return HandlerResult.Failure(Resources.Get("FilterNotFound"));

            return HandlerResult.Success();
        }
    }
}
