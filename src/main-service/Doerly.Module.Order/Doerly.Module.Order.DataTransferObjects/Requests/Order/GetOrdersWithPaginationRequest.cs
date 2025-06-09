using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class GetOrdersWithPaginationRequest : GetEntitiesWithPaginationRequest
{ 
    public int? CustomerId { get; set; }

    public int? ExecutorId { get; set; }
}
