using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.Contracts;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;

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
    
    
}
