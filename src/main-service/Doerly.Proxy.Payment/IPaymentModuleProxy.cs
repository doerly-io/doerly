using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Payments.DataTransferObjects;
using Doerly.Module.Payments.DataTransferObjects.Responses;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Payment;

public interface IPaymentModuleProxy : IModuleProxy
{
    Task<OperationResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
    
    Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request);

    Task<PaymentStatisticsDto> GetPaymentStatisticsAsync();
}
