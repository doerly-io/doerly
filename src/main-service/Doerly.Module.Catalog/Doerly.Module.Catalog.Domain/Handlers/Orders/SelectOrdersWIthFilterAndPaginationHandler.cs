using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Handlers;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Catalog.Domain.Handlers.Orders;

public class SelectOrdersWIthFilterAndPaginationHandler : BaseHandler
{
    private readonly IOrdersModuleProxy _ordersModuleProxy;

    public SelectOrdersWIthFilterAndPaginationHandler(IOrdersModuleProxy ordersModuleProxy)
    {
        _ordersModuleProxy = ordersModuleProxy;
    }

    public async Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> HandleAsync(
        GetOrderWithFilterAndPaginationRequest dto)
    {
        var response = await _ordersModuleProxy.GetOrdersWithPaginationAsync(dto);
        return response;
    }
}
