using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Contracts.Responses;

namespace Doerly.Proxy.Payment;

public interface IPaymentModuleWrapper
{
    Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);

    Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request);
    
    Task<PaymentStatisticsDto> GetPaymentStatisticsAsync();
}
