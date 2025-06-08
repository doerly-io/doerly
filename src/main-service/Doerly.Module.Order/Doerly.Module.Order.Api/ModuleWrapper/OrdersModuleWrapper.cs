using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Factories;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Domain.Handlers.Order;

namespace Doerly.Module.Order.Api.ModuleWrapper;

public interface IOrdersModuleWrapper
{
    Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(GetOrderWithFilterAndPaginationRequest request);
}

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
}
