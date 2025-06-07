using Doerly.Domain.Models;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.Handlers;

public class CheckoutHandler : BasePaymentHandler
{
    private readonly PaymentClientFactory _paymentClientFactory;
    private readonly ILogger<CheckoutHandler> _logger;

    public CheckoutHandler(
        PaymentDbContext dbContext,
        PaymentClientFactory paymentClientFactory,
        ILogger<CheckoutHandler> logger
    ) : base(dbContext)
    {
        _paymentClientFactory = paymentClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// 1.Check if billId is provided, if so, check for existing pending payment.
    /// 2. If a pending payment exists, return its checkout URL.
    /// 3. If no pending payment exists, check if the bill exists and is not already paid.
    /// 4. If the billId is not provided, create a new bill with the provided amount and description.
    /// </summary>
    public async Task<HandlerResult<BaseCheckoutResponse>> HandleAsync(CheckoutRequest checkoutRequest, Uri webhookUrl)
    {
        Bill bill;

        if (checkoutRequest.BillId.HasValue)
        {
            var existingPendingPayment = await DbContext.Payments
                .Where(p => p.BillId == checkoutRequest.BillId && p.Status == EPaymentStatus.Pending)
                .Select(x => new { x.Id, x.CheckoutUrl })
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(existingPendingPayment?.CheckoutUrl))
            {
                return HandlerResult.Success(new BaseCheckoutResponse
                {
                    CheckoutUrl = existingPendingPayment.CheckoutUrl,
                    BillId = checkoutRequest.BillId.Value,
                    Aggregator = EPaymentAggregator.LiqPay
                });
            }

            bill = await DbContext.Bills
                .FirstOrDefaultAsync(b => b.Id == checkoutRequest.BillId.Value);

            if (bill == null)
            {
                _logger.LogError("Bill with Id {BillId} not found", checkoutRequest.BillId);
                return HandlerResult.Failure<BaseCheckoutResponse>("BillNotFound");
            }

            if (bill.AmountPaid != bill.AmountTotal)
                return HandlerResult.Failure<BaseCheckoutResponse>("BillAlreadyPaid");
        }
        else
        {
            bill = new Bill
            {
                AmountTotal = checkoutRequest.AmountTotal,
                Description = checkoutRequest.BillDescription,
                PayerId = checkoutRequest.PayerId,
                PayerEmail = checkoutRequest.PayerEmail
            };
            DbContext.Bills.Add(bill);
        }

        var paymentGuid = Guid.CreateVersion7();

        var paymentClient = _paymentClientFactory(EPaymentAggregator.LiqPay);
        var checkoutResult = await paymentClient.Checkout(new CheckoutModel
        {
            Amount = checkoutRequest.AmountTotal,
            Currency = checkoutRequest.Currency,
            Description = checkoutRequest.BillDescription,
            PaymentId = paymentGuid.ToString(),
            BillId = bill.Id,
            PaymentAction = checkoutRequest.PaymentAction,
            ReturnUrl = checkoutRequest.ReturnUrl,
            CallbackUrl = webhookUrl.ToString(),
        });

        if (!checkoutResult.IsSuccess)
        {
            _logger.LogError("Checkout request failed, CheckoutRequestError: {CheckoutRequestError}",
                checkoutResult.ErrorMessage);
            return HandlerResult.Failure<BaseCheckoutResponse>("FailedToCreateCheckout");
        }

        var payment = new Payment
        {
            Guid = paymentGuid,
            Description = checkoutRequest.PaymentDescription,
            Currency = checkoutRequest.Currency,
            Amount = checkoutRequest.AmountTotal,
            Status = EPaymentStatus.Pending,
            Action = checkoutRequest.PaymentAction,
            Bill = bill,
            CheckoutUrl = checkoutResult.Value.CheckoutUrl,
        };

        DbContext.Payments.Add(payment);
        await DbContext.SaveChangesAsync();

        return HandlerResult.Success(new BaseCheckoutResponse
        {
            CheckoutUrl = payment.CheckoutUrl,
            BillId = bill.Id,
            Aggregator = EPaymentAggregator.LiqPay
        });
    }
}