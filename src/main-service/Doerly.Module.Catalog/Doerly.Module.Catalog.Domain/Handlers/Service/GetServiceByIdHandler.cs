using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.Contracts.Dtos.Responses.Service;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServiceByIdHandler : BaseCatalogHandler
    {
        public GetServiceByIdHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<HandlerResult<GetServiceResponse>> HandleAsync(int id)
        {
            var service = await DbContext.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return HandlerResult.Failure<GetServiceResponse>(Resources.Get("ServiceNotFound"));

            var serviceDto = new GetServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                CategoryId = service.CategoryId,
                CategoryName = service.Category?.Name,
                UserId = service.UserId,
                Price = service.Price,
                IsDeleted = service.IsDeleted,
                IsEnabled = service.IsEnabled
            };

            return HandlerResult.Success(serviceDto);
        }
    }
}
