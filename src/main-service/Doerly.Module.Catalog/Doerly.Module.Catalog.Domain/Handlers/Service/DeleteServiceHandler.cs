using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class DeleteServiceHandler : BaseCatalogHandler
    {
        public DeleteServiceHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult> HandleAsync(int id)
        {
            var service = await DbContext.Services.FindAsync(id);
            if (service == null)
                return HandlerResult.Failure(Resources.Get("ServiceNotFound"));

            if (service.IsDeleted)
                return HandlerResult.Failure(Resources.Get("ServiceAlreadyDeleted"));

            service.IsDeleted = true;
            await DbContext.SaveChangesAsync();

            return HandlerResult.Success();
        }
    }
}
