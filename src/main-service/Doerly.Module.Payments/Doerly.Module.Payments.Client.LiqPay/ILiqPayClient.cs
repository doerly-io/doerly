using Doerly.Module.Payments.Client.LiqPay;

namespace Doerly.Module.Payments.Client.LiqPay;

public interface ILiqPayClient
{
    LiqPayRequest BuildCheckoutData(LiqPayCheckout liqPayCheckout);
    
    CheckoutResponse ValidateSignature(string data, string signature);
}
