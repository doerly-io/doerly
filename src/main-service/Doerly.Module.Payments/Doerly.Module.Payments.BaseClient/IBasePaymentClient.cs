using Doerly.Module.Payments.Contracts;

namespace Doerly.Module.Payments.BaseClient;

public interface IBasePaymentClient
{
    Task<BaseCheckoutResponse> Checkout(CheckoutModel checkoutModel);

    bool ValidateSignature(string data, string signature);

    string GenerateSignature(string data);

}
