using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Payments.DataTransferObjects;
using Doerly.Module.Payments.DataTransferObjects.Responses;

namespace Doerly.Module.Payments.Contracts;

public interface IPaymentModuleWrapper
{
    Task<OperationResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);

    Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request);
    
    Task<PaymentStatisticsDto> GetPaymentStatisticsAsync();
}