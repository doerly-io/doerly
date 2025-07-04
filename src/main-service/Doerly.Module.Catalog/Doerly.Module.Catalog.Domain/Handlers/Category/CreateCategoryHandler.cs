﻿using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;

namespace Doerly.Module.Catalog.Domain.Handlers.Category
{
    public class CreateCategoryHandler : BaseCatalogHandler
    {
        public CreateCategoryHandler(CatalogDbContext dbContext) : base(dbContext) { }

        public async Task<OperationResult<int>> HandleAsync(CreateCategoryRequest request)
        {
            if (request.ParentId.HasValue)
            {
                var parentExists = await DbContext.Categories.AnyAsync(c => c.Id == request.ParentId);
                if (!parentExists)
                    return OperationResult.Failure<int>(Resources.Get("ParentCategoryNotFound"));
            }

            var category = new CategoryEntity
            {
                Name = request.Name,
                Description = request.Description,
                ParentId = request.ParentId,
                IsEnabled = request.IsEnabled,
            };

            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();

            return OperationResult.Success(category.Id);
        }
    }

}
