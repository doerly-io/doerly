using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Factories;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Contracts.Dtos.Responses;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Order.Domain.Handlers.Order;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Order.Api.ModuleWrapper;


public class OrdersModuleWrapper : IOrdersModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public OrdersModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(GetOrderWithFilterAndPaginationRequest request)
    {
        var ordersResponse = await _handlerFactory.Get<SelectOrdersWithFilterAndPaginationHandler>().HandleAsync(request);
        return ordersResponse;
    }

    public Task<CursorPaginationResponse<OrderFeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request)
    {
        var feedbacksResponse = _handlerFactory.Get<SelectUserFeedbacksHandler>().HandleAsync(userId, request);
        return feedbacksResponse;
    }
}
