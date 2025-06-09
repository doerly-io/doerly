using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class GetExecutionProposalsWithPaginationRequest : GetEntitiesWithPaginationRequest
{
    public int? ReceiverId { get; set; }

    public int? SenderId { get; set; }
}
