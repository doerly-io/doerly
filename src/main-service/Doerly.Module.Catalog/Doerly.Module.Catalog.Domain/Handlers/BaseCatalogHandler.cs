using Doerly.Domain.Handlers;
using Doerly.Module.Catalog.DataAccess;

namespace Doerly.Module.Catalog.Domain.Handlers
{
    public class BaseCatalogHandler : BaseHandler<CatalogDbContext>
    {
        public BaseCatalogHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
