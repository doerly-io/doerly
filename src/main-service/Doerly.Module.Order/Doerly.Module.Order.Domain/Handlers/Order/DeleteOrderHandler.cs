using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Enums;
using Doerly.Module.Order.Localization;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class DeleteOrderHandler : BaseOrderHandler
{
    public DeleteOrderHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult> HandleAsync(int id)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return HandlerResult.Failure(Resources.Get("ORDER_NOT_FOUND"));

        order.Status = OrderStatus.Canceled;
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
