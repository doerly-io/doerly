using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Orders;

public interface IOrdersModuleProxy : IModuleProxy
{
    Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(
        GetOrderWithFilterAndPaginationRequest request);
}
