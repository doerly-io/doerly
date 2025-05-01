using Doerly.Module.Payments.Client.LiqPay.Internal.Models;

namespace Doerly.Module.Payments.Client.LiqPay.Internal;

internal static class DtoMappingExtensions
{
    internal static LiqPayCheckoutRequest ToDto(this LiqPayCheckout liqPayCheckout, string publicKey)
    {
        return new LiqPayCheckoutRequest()
        {
            Amount = liqPayCheckout.Amount,
            Currency = liqPayCheckout.Currency,
            Description = liqPayCheckout.Description,
            OrderId = liqPayCheckout.OrderId,
            ApiVersion = liqPayCheckout.ApiVersion,
            PaymentAction = liqPayCheckout.PaymentAction,
            ResultUrl = liqPayCheckout.ResultUrl,
            ServerUrl = liqPayCheckout.ServerUrl,
            PublicKey = publicKey
        };
    }
    
}