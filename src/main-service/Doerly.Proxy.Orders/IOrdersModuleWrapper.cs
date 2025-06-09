using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Contracts.Dtos.Responses;

namespace Doerly.Proxy.Orders;

public interface IOrdersModuleWrapper
{
    Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(GetOrderWithFilterAndPaginationRequest request);

    Task<CursorPaginationResponse<OrderFeedbackResponse>> HandleAsync(int userId, CursorPaginationRequest request);
}
