using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.Domain.Dtos.Responses.Service;
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
            var service = await DbContext.Services.FindAsync(id);
            if (service == null)
                return HandlerResult.Failure<GetServiceResponse>(Resources.Get("SERVICE_NOT_FOUND"));

            var serviceDto = new GetServiceResponse
            {
                Id = order.Id,
                CategoryId = order.CategoryId,
                Name = order.Name,
                Description = order.Description,
                Price = order.Price,
                PaymentKind = order.PaymentKind,
                DueDate = order.DueDate,
                Status = order.Status,
                CustomerId = order.CustomerId,
                ExecutorId = order.ExecutorId,
                ExecutionDate = order.ExecutionDate,
                BillId = order.BillId
            };

            return HandlerResult.Success(serviceDto);
        }
    }
}
