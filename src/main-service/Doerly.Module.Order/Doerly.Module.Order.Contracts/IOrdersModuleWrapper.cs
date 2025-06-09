using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Requests;

namespace Doerly.Module.Order.Contracts;

public interface IOrdersModuleWrapper
{
    Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> GetOrdersWithPaginationAsync(GetOrderWithFilterAndPaginationRequest request);
}
