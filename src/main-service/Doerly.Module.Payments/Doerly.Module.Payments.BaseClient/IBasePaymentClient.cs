using Doerly.Domain.Models;
using Doerly.Module.Payments.DataTransferObjects;

namespace Doerly.Module.Payments.BaseClient;

public interface IBasePaymentClient
{
    Task<OperationResult<BaseCheckoutResponse>> Checkout(CheckoutModel checkoutModel);

    bool ValidateSignature(string data, string signature);

    string GenerateSignature(string data);

    Task<OperationResult> TransferToCard(TransferModel transferModel);

}
