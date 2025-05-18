using Doerly.Domain.Models;
using Doerly.Module.Payments.Api.ModuleWrapper;
using Doerly.Module.Payments.Contracts;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Payment;

public interface IPaymentModuleProxy : IModuleProxy
{
    Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
}

public class PaymentModuleProxy : IPaymentModuleProxy
{
    private readonly IPaymentModuleWrapper _paymentModuleWrapper;

    public PaymentModuleProxy(IPaymentModuleWrapper paymentModuleWrapper)
    {
        _paymentModuleWrapper = paymentModuleWrapper;
    }

    public async Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest)
    {
        var checkoutResponse = await _paymentModuleWrapper.CheckoutAsync(checkoutRequest);
        return checkoutResponse;
    }
}
