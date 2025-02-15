using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Requests.Order;
using Doerly.Module.Order.Domain.Dtos.Responses.Order;
using Doerly.Module.Order.Localization;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class UpdateOrderHandler : BaseOrderHandler
{
    public UpdateOrderHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<HandlerResult> HandleAsync(UpdateOrderRequest dto)
    {
        var order = await DbContext.Orders.FindAsync(dto.Id);
        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("ORDER_NOT_FOUND"));

        order.CategoryId = dto.CategoryId;
        order.Name = dto.Name;
        order.Description = dto.Description;
        order.Price = dto.Price;
        order.PaymentKind = dto.PaymentKind;
        order.DueDate = dto.DueDate;

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
