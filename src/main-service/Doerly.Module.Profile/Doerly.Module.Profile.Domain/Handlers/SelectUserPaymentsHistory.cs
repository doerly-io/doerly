using Doerly.DataTransferObjects;
using Doerly.Domain;
using Doerly.Domain.Handlers;
using Doerly.Module.Payments.Contracts;
using Doerly.Proxy.Payment;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SelectUserPaymentsHistory : BaseHandler
{
    private readonly IDoerlyRequestContext _requestContext;
    private readonly IPaymentModuleProxy _paymentModuleProxy;

    public SelectUserPaymentsHistory(IDoerlyRequestContext requestContext, IPaymentModuleProxy paymentModuleProxy)
    {
        _requestContext = requestContext;
        _paymentModuleProxy = paymentModuleProxy;
    }

    public async Task<CursorPaginationResponse<PaymentHistoryItemResponse>> HandleAsync(CursorPaginationRequest request)
    {
        var userId = _requestContext.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException();

        var paymentsHistory = await _paymentModuleProxy.GetUserPayments(userId.Value, request);
        return paymentsHistory;
    }
}
