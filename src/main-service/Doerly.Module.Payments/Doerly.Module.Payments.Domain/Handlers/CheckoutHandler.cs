using Doerly.Domain.Models;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts.Requests;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataAccess.Models;
using Doerly.Module.Payments.Client.LiqPay.Client;

namespace Doerly.Module.Payments.Domain.Handlers;

public class CheckoutHandler : BasePaymentHandler
{
    private readonly ILiqPayClient _liqPayClient;

    public CheckoutHandler(PaymentDbContext dbContext, ILiqPayClient liqPayClient) : base(dbContext)
    {
        _liqPayClient = liqPayClient;
    }

    public async Task<HandlerResult> HandlePaymentAsync(CreateInvoiceRequest createInvoiceRequest)
    {
        var invoice = new Invoice
        {
            AmountTotal = createInvoiceRequest.AmountTotal,
        };
        
        DbContext.Invoices.Add(invoice);
        await DbContext.SaveChangesAsync();
        
        
    }
}
