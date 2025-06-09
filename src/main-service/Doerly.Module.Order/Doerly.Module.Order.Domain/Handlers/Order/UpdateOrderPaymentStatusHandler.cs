using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class UpdateOrderPaymentStatusHandler : BaseOrderHandler
{
    public UpdateOrderPaymentStatusHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<OperationResult> HandleAsync(int billId, bool isPaid)
    {
        var order = await DbContext.Orders.Select(x => x.BillId).AnyAsync(x => x == billId);
        if (!order)
            return OperationResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        if (isPaid)
        {
            await DbContext.Orders.Where(x => x.BillId == billId && x.Status == EOrderStatus.AwaitingPayment)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(order => order.CustomerCompletionConfirmed, true)
                    .SetProperty(order => order.Status, order => order.ExecutorCompletionConfirmed
                        ? EOrderStatus.Completed : EOrderStatus.AwaitingConfirmation));
        }

        return OperationResult.Success();
    }
}
