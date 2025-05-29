using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay.Helpers;
using Doerly.Module.Payments.Client.LiqPay.Internal.Models;

namespace Doerly.Module.Payments.Client.LiqPay.Internal;

internal static class DtoMappingExtensions
{
    internal static LiqPayCheckoutRequest ToDto(this CheckoutModel liqPayCheckout, string publicKey)
    {
        return new LiqPayCheckoutRequest
        {
            Amount = liqPayCheckout.Amount,
            Currency = liqPayCheckout.Currency,
            Description = liqPayCheckout.Description,
            OrderId = liqPayCheckout.PaymentId.ToString(),
            ApiVersion = liqPayCheckout.ApiVersion,
            PaymentAction = LiqPayMappingHelper.MapCommonActionToLiqPayAction(liqPayCheckout.PaymentAction),
            ResultUrl = liqPayCheckout.ReturnUrl,
            ServerUrl = liqPayCheckout.CallbackUrl,
            PublicKey = publicKey
        };
    }
    
}
