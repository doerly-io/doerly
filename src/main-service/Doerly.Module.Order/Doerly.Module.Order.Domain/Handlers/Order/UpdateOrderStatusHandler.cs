using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Enums;
using Doerly.Proxy.Payment;
using Doerly.Domain;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class UpdateOrderStatusHandler : BaseOrderHandler
{
    private readonly IPaymentModuleProxy _paymentModuleProxy;
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public UpdateOrderStatusHandler(OrderDbContext dbContext, IPaymentModuleProxy paymentModuleProxy, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _paymentModuleProxy = paymentModuleProxy;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult<UpdateOrderStatusResponse>> HandleAsync(int id, UpdateOrderStatusRequest dto)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return HandlerResult.Failure<UpdateOrderStatusResponse>(Resources.Get("OrderNotFound"));

        if (_doerlyRequestContext.UserId == order.CustomerId)
        {
            dto.CustomerId = _doerlyRequestContext.UserId ?? dto.CustomerId;
            dto.ExecutorId = null;
        }
        else if (_doerlyRequestContext.UserId == order.ExecutorId)
        {
            dto.ExecutorId = _doerlyRequestContext.UserId ?? dto.ExecutorId;
            dto.CustomerId = null;
        }

        /* the logic of the order status change is as follows:
         * the customer can change order's status to canceled when it is placed
         * the customer can mark order's completion when it is in progress
         * the executor can mark order's completion when it is in progress
         * when both customer and executor mark order's completion, the order's status changes to completed
         */
        var result = new UpdateOrderStatusResponse();
        if (dto.CustomerId == order.CustomerId)
        {
            if (order.Status == EOrderStatus.Placed && dto.Status == EOrderStatus.Canceled)
                order.Status = dto.Status;

            else if ((order.Status == EOrderStatus.InProgress || order.Status == EOrderStatus.AwaitingPayment) && dto.Status == EOrderStatus.Completed)
            {
                if (order.PaymentKind == EPaymentKind.Online)
                {
                    var checkoutRequest = new CheckoutRequest()
                    {
                        AmountTotal = order.Price,
                        PayerId = order.CustomerId,
                        BillDescription = string.Empty, //hardcoded for now
                        PaymentDescription = string.Format(Resources.Get("OrderPaymentDescription"), order.Id), //hardcoded for now
                        Currency = ECurrency.UAH, //hardcoded for now
                        PaymentAction = EPaymentAction.Pay,
                        ReturnUrl = dto.ReturnUrl,
                    };
                    var payment = await _paymentModuleProxy.CheckoutAsync(checkoutRequest);
                    result.PaymentUrl = payment.Value.CheckoutUrl;
                    order.BillId = payment.Value.BillId;
                    order.Status = EOrderStatus.AwaitingPayment;
                }
                else
                {
                    order.CustomerCompletionConfirmed = true;
                    order.Status = EOrderStatus.AwaitingConfirmation;
                }
            }
        } 
        else if (order.ExecutorId != null && dto.ExecutorId == order.ExecutorId)
        {
            if ((order.Status == EOrderStatus.InProgress || order.Status == EOrderStatus.AwaitingPayment 
                || order.Status == EOrderStatus.AwaitingConfirmation)
                && dto.Status == EOrderStatus.Completed && order.ExecutorCompletionConfirmed == false)
            {
                if (order.PaymentKind == EPaymentKind.Online)
                    order.Status = EOrderStatus.AwaitingPayment;
                else
                    order.Status = EOrderStatus.AwaitingConfirmation;

                order.ExecutorCompletionConfirmed = true;
            }
        }

        if (order.CustomerCompletionConfirmed && order.ExecutorCompletionConfirmed)
        {
            order.Status = EOrderStatus.Completed;
            order.ExecutionDate = DateTime.UtcNow;
        }

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success(result);
    }
}
