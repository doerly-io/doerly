using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class CancelOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;
    public CancelOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id)
    {
        var order = await DbContext.Orders.Select(x => new { x.Id, x.CustomerId })
            .AnyAsync(x => x.Id == id && x.CustomerId == _doerlyRequestContext.UserId);
        if (!order)
            return HandlerResult.Failure(Resources.Get("OrderNotFound"));

        await DbContext.Orders.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(order => order.Status, EOrderStatus.Canceled));

        return HandlerResult.Success();
    }
}
