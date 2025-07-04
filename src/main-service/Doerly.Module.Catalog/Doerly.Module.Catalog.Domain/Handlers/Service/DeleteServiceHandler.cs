﻿using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class DeleteServiceHandler : BaseCatalogHandler
    {
        public DeleteServiceHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> HandleAsync(int id)
        {
            var updatedCount = await DbContext.Services
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsDeleted, true));

            if (updatedCount == 0)
                return OperationResult.Failure(Resources.Get("ServiceNotFound"));

            return OperationResult.Success();
        }

    }
}
