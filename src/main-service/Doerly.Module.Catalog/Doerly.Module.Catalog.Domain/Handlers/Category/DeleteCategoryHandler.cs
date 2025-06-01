using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class DeleteCategoryHandler : BaseCatalogHandler
    {
        public DeleteCategoryHandler(CatalogDbContext dbContext) : base(dbContext) { }

        public async Task<HandlerResult> HandleAsync(int id)
        {
            var category = await DbContext.Categories.FindAsync(id);
            if (category == null)
                return HandlerResult.Failure(Resources.Get("CategoryNotFound"));

            if (category.IsDeleted)
                return HandlerResult.Failure(Resources.Get("CategoryAlreadyDeleted"));

            category.IsDeleted = true;
            await DbContext.SaveChangesAsync();

            return HandlerResult.Success();
        }
    }
}
