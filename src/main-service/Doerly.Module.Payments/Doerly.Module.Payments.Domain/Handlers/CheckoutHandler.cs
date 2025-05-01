using Doerly.Domain.Models;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts.Requests;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;

namespace Doerly.Module.Payments.Domain.Handlers;

public class CheckoutHandler : BasePaymentHandler
{
    private readonly ILiqPayClient _liqPayClient;

    public CheckoutHandler(PaymentDbContext dbContext, ILiqPayClient liqPayClient) : base(dbContext)
    {
        _liqPayClient = liqPayClient;
    }

    public async Task<HandlerResult<LiqPayRequest>> HandlePaymentAsync(CreateInvoiceRequest createInvoiceRequest, Uri webhookUri)
    {
        var invoice = new Invoice
        {
            AmountTotal = createInvoiceRequest.AmountTotal,
        };
        
        DbContext.Invoices.Add(invoice);
        await DbContext.SaveChangesAsync();

        var liqPayRequest = _liqPayClient.BuildCheckoutData(new LiqPayCheckout
        {
            Amount = createInvoiceRequest.AmountTotal,
            Currency = LiqPayConstants.CurrenciesConstants.UAH,
            Description = createInvoiceRequest.Description,
            OrderId = createInvoiceRequest.OrderGuid.ToString(),
            PaymentAction = LiqPayConstants.PaymentActionConstants.Pay,
            ResultUrl = createInvoiceRequest.ReturnUrl,
            ServerUrl = webhookUri.ToString(),
        });

        return HandlerResult.Success(liqPayRequest);
    }
}
