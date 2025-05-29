using Doerly.Domain.Models;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Enums;
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

    public async Task<HandlerResult<BaseCheckoutResponse>> HandleAsync(CheckoutRequest checkoutRequest, Uri webhookUrl)
    {
        var bill = new Bill
        {
            AmountTotal = checkoutRequest.AmountTotal,
            Description = checkoutRequest.BillDescription,
            PayerId = checkoutRequest.PayerId,
        };

        var payment = new Payment
        {
            Description = checkoutRequest.PaymentDescription,
            Currency = checkoutRequest.Currency,
            Amount = checkoutRequest.AmountTotal,
            Status = EPaymentStatus.Pending,
            Action = checkoutRequest.PaymentAction,
            Bill = bill,
        };

        DbContext.Bills.Add(bill);
        DbContext.Payments.Add(payment);
        await DbContext.SaveChangesAsync();

        var paymentClient = _paymentClientFactory(EPaymentAggregator.LiqPay); //TODO: remove hardcode after adding other payment methods
        var checkoutResult = await paymentClient.Checkout(new CheckoutModel
        {
            Amount = checkoutRequest.AmountTotal,
            Currency = CurrencyToStringCode(checkoutRequest.Currency),
            Description = checkoutRequest.BillDescription,
            PaymentId = payment.Id,
            BillId = bill.Id,
            PaymentAction = checkoutRequest.PaymentAction,
            ReturnUrl = checkoutRequest.ReturnUrl,
            CallbackUrl = webhookUrl.ToString(),
        });

        if (checkoutResult.IsSuccess)
            return checkoutResult;

        _logger.LogError("Checkout request failed, CheckoutRequestError: {CheckoutRequestError}", checkoutResult.ErrorMessage);
        return HandlerResult.Failure<BaseCheckoutResponse>("FailedToCreateCheckout");
    }

    private string CurrencyToStringCode(ECurrency currency) => currency switch
    {
        ECurrency.UAH => LiqPayConstants.CurrenciesConstants.UAH,
        ECurrency.USD => LiqPayConstants.CurrenciesConstants.USD,
        ECurrency.EUR => LiqPayConstants.CurrenciesConstants.EUR,
        _ => LiqPayConstants.CurrenciesConstants.UAH
    };
}
