using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Handlers;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Responses;
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
