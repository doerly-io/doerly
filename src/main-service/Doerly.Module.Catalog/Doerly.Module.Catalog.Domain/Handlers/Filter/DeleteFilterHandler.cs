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

        public async Task<OperationResult> HandleAsync(int id)
        {
            var deletedCount = await DbContext.Filters
                .Where(f => f.Id == id)
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
                return OperationResult.Failure(Resources.Get("FilterNotFound"));

            return OperationResult.Success();
        }
    }
}
