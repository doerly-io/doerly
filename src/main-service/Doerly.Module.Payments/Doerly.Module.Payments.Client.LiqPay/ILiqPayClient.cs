using Doerly.Module.Payments.Client.LiqPay;

namespace Doerly.Module.Payments.Client.LiqPay;

public interface ILiqPayClient
{
    CheckoutResponse ValidateSignature(string data, string signature);
}
