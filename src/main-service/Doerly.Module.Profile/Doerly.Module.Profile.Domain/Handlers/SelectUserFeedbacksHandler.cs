using Doerly.DataTransferObjects;
using Doerly.Domain.Handlers;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Profile.Domain.Handlers;

public class SelectUserFeedbacksHandler : BaseHandler
{
    private readonly IOrdersModuleProxy _ordersModuleProxy;

    public SelectUserFeedbacksHandler(IOrdersModuleProxy ordersModuleProxy)
    {
        _ordersModuleProxy = ordersModuleProxy;
    }

    public async Task<CursorPaginationResponse<OrderFeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request)
    {
        var response = await _ordersModuleProxy.GetFeedbacksAsync(userId, request);
        return response;
    }
}
