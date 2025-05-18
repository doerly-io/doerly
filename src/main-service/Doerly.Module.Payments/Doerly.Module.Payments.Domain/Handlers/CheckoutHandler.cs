using Doerly.Common.Settings;
using Doerly.Domain.Models;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Payments.Domain.Handlers;

public class CheckoutHandler : BasePaymentHandler
{
    private readonly PaymentClientFactory _paymentClientFactory;

    public CheckoutHandler(
        PaymentDbContext dbContext,
        PaymentClientFactory paymentClientFactory
    ) : base(dbContext)
    {
        _paymentClientFactory = paymentClientFactory;
    }

    public async Task<HandlerResult<BaseCheckoutResponse>> HandleAsync(CheckoutRequest checkoutRequest, Uri webhookUrl)
    {
        var bill = new Bill
        {
            AmountTotal = checkoutRequest.AmountTotal,
            Description = checkoutRequest.Description,
            PayerId = checkoutRequest.PayerId,
        };

        var payment = new Payment
        {
            Description = checkoutRequest.Description,
            Currency = checkoutRequest.Currency,
            Amount = checkoutRequest.AmountTotal,
            Status = EPaymentStatus.Pending,
            Action = EPaymentAction.Pay, //TODO: remove hardcode after adding other payment methods
            Bill = bill,
        };

        DbContext.Bills.Add(bill);
        DbContext.Payments.Add(payment);
        await DbContext.SaveChangesAsync();

        var paymentClient = _paymentClientFactory(EPaymentAggregator.LiqPay); //TODO: remove hardcode after adding other payment methods
        var checkoutResponse = await paymentClient.Checkout(new CheckoutModel
        {
            Amount = checkoutRequest.AmountTotal,
            Currency = CurrencyToStringCode(checkoutRequest.Currency),
            Description = checkoutRequest.Description,
            BillId = bill.Id,
            PaymentAction = EPaymentAction.Pay,
            ReturnUrl = checkoutRequest.ReturnUrl,
            CallbackUrl = webhookUrl.ToString(),
        });

        if (string.IsNullOrEmpty(checkoutResponse.CheckoutUrl))
            return HandlerResult.Failure<BaseCheckoutResponse>("Failed to create checkout URL");

        return HandlerResult.Success(checkoutResponse);
    }

    private string CurrencyToStringCode(ECurrency currency) => currency switch
    {
        ECurrency.UAH => LiqPayConstants.CurrenciesConstants.UAH,
        ECurrency.USD => LiqPayConstants.CurrenciesConstants.USD,
        ECurrency.EUR => LiqPayConstants.CurrenciesConstants.EUR,
        _ => LiqPayConstants.CurrenciesConstants.UAH
    };
}
