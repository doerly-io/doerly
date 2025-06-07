using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Payments.Contracts;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Payment;

public interface IPaymentModuleProxy : IModuleProxy
{
    Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
    
    Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request);

}
