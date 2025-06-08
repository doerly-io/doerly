using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Handlers;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Proxy.Orders;

namespace Doerly.Module.Catalog.Domain.Handlers.Orders;

public class SelectOrdersWithFilterAndPaginationHandler : BaseHandler
{
    private readonly IOrdersModuleProxy _ordersModuleProxy;

    public SelectOrdersWithFilterAndPaginationHandler(IOrdersModuleProxy ordersModuleProxy)
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
