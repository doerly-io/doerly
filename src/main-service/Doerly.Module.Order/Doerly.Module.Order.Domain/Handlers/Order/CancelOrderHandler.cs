using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Enums;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class CancelOrderHandler : BaseOrderHandler
{
    public CancelOrderHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult> HandleAsync(int id)
    {
        var order = await DbContext.Orders.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);
        if (order == 0)
            return HandlerResult.Failure(Resources.Get("ORDER_NOT_FOUND"));

        await DbContext.Orders.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(order => order.Status, EOrderStatus.Canceled));

        return HandlerResult.Success();
    }
}
