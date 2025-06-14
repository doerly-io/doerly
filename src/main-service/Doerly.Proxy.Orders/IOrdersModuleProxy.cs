using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Proxy.Orders;

public interface IOrdersModuleProxy : IModuleProxy
{
    Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(
        GetOrderWithFilterAndPaginationRequest request);

    Task<CursorPaginationResponse<OrderFeedbackResponse>> GetFeedbacksAsync(int userId, CursorPaginationRequest request);

    Task<List<GetOrdersAmountByCategoriesResponse>> GetOrdersAmountByCategoriesAsync();
}
