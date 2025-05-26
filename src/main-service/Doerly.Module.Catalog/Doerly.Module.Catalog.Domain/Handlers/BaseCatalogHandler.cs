using Doerly.Domain.Handlers;
using Doerly.Module.Catalog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers
{
    public class BaseCatalogHandler : BaseHandler<CatalogDbContext>
    {
        public BaseCatalogHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
