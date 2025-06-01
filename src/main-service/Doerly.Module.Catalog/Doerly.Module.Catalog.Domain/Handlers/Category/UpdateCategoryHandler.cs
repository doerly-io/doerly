using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Category;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class UpdateCategoryHandler : BaseCatalogHandler
    {
        public UpdateCategoryHandler(CatalogDbContext dbContext) : base(dbContext) 
        { 
        }

        public async Task<HandlerResult> HandleAsync(UpdateCategoryRequest request)
        {
            var category = await DbContext.Categories.FindAsync(request.Id);
            if (category == null)
                return HandlerResult.Failure(Resources.Get("CategoryNotFound"));

            if (request.ParentId == request.Id)
                return HandlerResult.Failure(Resources.Get("CategoryCannotBeOwnParent"));

            if (request.ParentId.HasValue)
            {
                var parentExists = await DbContext.Categories.AnyAsync(c => c.Id == request.ParentId);
                if (!parentExists)
                    return HandlerResult.Failure(Resources.Get("ParentCategoryNotFound"));
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.ParentId = request.ParentId;
            category.IsEnabled = request.IsEnabled;

            await DbContext.SaveChangesAsync();

            return HandlerResult.Success();
        }
    }
}
