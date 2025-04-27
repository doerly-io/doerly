using Doerly.Domain.Handlers;
using Doerly.Module.Payments.Client.LiqPay;

namespace Doerly.Module.Payments.Domain.WebhookHandlers;

public class LiqPayFinalStatus : BaseHandler
{
    private readonly ILiqPayClient _liqPayClient;

    public LiqPayFinalStatus(ILiqPayClient liqPayClient)
    {
        _liqPayClient = liqPayClient;
    }
    
    public Task HandleAsync(string data, string signature)
    {
        var result = _liqPayClient.ValidateSignature(data, signature);
        
        
    }
}
