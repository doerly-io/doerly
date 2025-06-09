using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.DataTransferObjects.Dtos;
using Doerly.Module.Payments.Enums;
using Doerly.Proxy.Payment;
using Doerly.Domain;
using Doerly.Messaging;
using Doerly.Module.Payments.DataTransferObjects;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class UpdateOrderStatusHandler : BaseOrderHandler
{
    private readonly IPaymentModuleProxy _paymentModuleProxy;
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public UpdateOrderStatusHandler(OrderDbContext dbContext, IPaymentModuleProxy paymentModuleProxy, 
        IDoerlyRequestContext doerlyRequestContext, IMessagePublisher messagePublisher) : base(dbContext, messagePublisher)
    {
        _paymentModuleProxy = paymentModuleProxy;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<OperationResult<UpdateOrderStatusResponse>> HandleAsync(int id, UpdateOrderStatusRequest dto)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return OperationResult.Failure<UpdateOrderStatusResponse>(Resources.Get("OrderNotFound"));

        /* the logic of the order status change is as follows:
         * the customer can change order's status to canceled when it is placed
         * the customer can mark order's completion when it is in progress
         * the executor can mark order's completion when it is in progress
         * when both customer and executor mark order's completion, the order's status changes to completed
         */
        var result = new UpdateOrderStatusResponse();
        if (_doerlyRequestContext.UserId == order.CustomerId)
        {
            if (order.Status == EOrderStatus.Placed && dto.Status == EOrderStatus.Canceled)
                order.Status = dto.Status;

            else if ((order.Status == EOrderStatus.InProgress || order.Status == EOrderStatus.AwaitingPayment) && dto.Status == EOrderStatus.Completed)
            {
                if (order.PaymentKind == EPaymentKind.Online)
                {
                    var checkoutRequest = new CheckoutRequest()
                    {
                        BillId = order.BillId,
                        AmountTotal = order.Price,
                        PayerId = order.CustomerId,
                        BillDescription = string.Empty, //hardcoded for now
                        PaymentDescription = string.Format(Resources.Get("OrderPaymentDescription"), order.Id), //hardcoded for now
                        Currency = ECurrency.UAH, //hardcoded for now
                        PaymentAction = EPaymentAction.Pay,
                        ReturnUrl = dto.ReturnUrl,
                        PayerEmail = _doerlyRequestContext.UserEmail
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
        else if (order.ExecutorId != null && _doerlyRequestContext.UserId == order.ExecutorId)
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

        await PublishOrderStatusUpdatedEventAsync(order.Id, order.Status);

        return OperationResult.Success(result);
    }
}
