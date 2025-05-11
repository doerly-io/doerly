using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Requests;
using Doerly.Module.Order.Domain.Dtos.Responses;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateOrderHandler : BaseOrderHandler
{
    public UpdateOrderHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<HandlerResult> HandleAsync(int id, UpdateOrderRequest dto)
    {
        var order = await DbContext.Orders.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);
        if (order == 0)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("ORDER_NOT_FOUND"));

        await DbContext.Orders.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(order => order.CategoryId, dto.CategoryId)
            .SetProperty(order => order.Name, dto.Name)
            .SetProperty(order => order.Description, dto.Description)
            .SetProperty(order => order.Price, dto.Price)
            .SetProperty(order => order.PaymentKind, dto.PaymentKind)
            .SetProperty(order => order.DueDate, dto.DueDate));

        return HandlerResult.Success();
    }
}
