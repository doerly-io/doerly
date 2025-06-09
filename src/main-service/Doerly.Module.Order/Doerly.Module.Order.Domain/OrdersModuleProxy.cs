using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Factories;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Order.Domain;

public class OrdersModuleProxy : IOrdersModuleProxy
{
    private readonly IHandlerFactory _handlerFactory;

    public OrdersModuleProxy(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(
        GetOrderWithFilterAndPaginationRequest request)
    {
        var ordersResponse = await _handlerFactory.Get<SelectOrdersWithFilterAndPaginationHandler>().HandleAsync(request);
        return ordersResponse;
    }

    public async Task<CursorPaginationResponse<OrderFeedbackResponse>> GetFeedbacksAsync(int userId, CursorPaginationRequest request)
    {
        var feedbacksResponse = await _handlerFactory.Get<SelectUserFeedbacksHandler>().HandleAsync(userId, request);
        return feedbacksResponse;
    }
}
