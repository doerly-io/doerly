using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Responses.Order;
using Doerly.Module.Order.Localization;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrderByIdHandler : BaseOrderHandler
{
    public GetOrderByIdHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<HandlerResult<GetOrderResponse>> HandleAsync(int id)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("ORDER_NOT_FOUND"));

        var orderDto = new GetOrderResponse
        {
            CategoryId = order.CategoryId,
            Id = order.Id,
            Name = order.Name,
            Description = order.Description,
            Price = order.Price,
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate,
            CustomerId = order.CustomerId,
            ExecutorId = order.ExecutorId,
            ExecutionDate = order.ExecutionDate,
            BillId = order.BillId
        };

        return HandlerResult.Success(orderDto);
    }
}
