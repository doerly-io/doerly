using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class UpdateOrderPaymentStatusHandler : BaseOrderHandler
{
    public UpdateOrderPaymentStatusHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult> HandleAsync(int billId, bool isPaid)
    {
        var order = await DbContext.Orders.Select(x => x.BillId).AnyAsync(x => x == billId);
        if (!order)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        if (isPaid)
        {
            await DbContext.Orders.Where(x => x.BillId == billId && x.Status == EOrderStatus.AwaitingPayment)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(order => order.CustomerCompletionConfirmed, true)
                    .SetProperty(order => order.Status, order => order.ExecutorCompletionConfirmed
                        ? EOrderStatus.Completed : EOrderStatus.AwaitingConfirmation));
        }

        return HandlerResult.Success();
    }
}
