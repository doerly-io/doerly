using Doerly.Domain.Models;
using Doerly.Module.Payments.Contracts;

namespace Doerly.Module.Payments.BaseClient;

public interface IBasePaymentClient
{
    Task<HandlerResult<BaseCheckoutResponse>> Checkout(CheckoutModel checkoutModel);

    bool ValidateSignature(string data, string signature);

    string GenerateSignature(string data);

}
