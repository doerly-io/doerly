using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Contracts.Dtos.Responses;

namespace Doerly.Proxy.Orders;

public class OrdersModuleProxy : IOrdersModuleProxy
{
    private readonly IOrdersModuleWrapper _ordersModuleWrapper;

    public OrdersModuleProxy(IOrdersModuleWrapper ordersModuleWrapper)
    {
        _ordersModuleWrapper = ordersModuleWrapper;
    }

    public async Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(
        GetOrderWithFilterAndPaginationRequest request)
    {
        var ordersResponse = await _ordersModuleWrapper.GetOrdersWithPaginationAsync(request);
        return ordersResponse;
    }

    public async Task<CursorPaginationResponse<OrderFeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request)
    {
        var feedbacksResponse = await _ordersModuleWrapper.HandleAsync(userId, request);
        return feedbacksResponse;
    }
}
