using Doerly.Domain.Factories;

namespace Doerly.Module.Order.Contracts;

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
