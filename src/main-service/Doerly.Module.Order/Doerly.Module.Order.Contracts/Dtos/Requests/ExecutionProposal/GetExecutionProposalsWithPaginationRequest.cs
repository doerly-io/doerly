using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Order.Contracts.Dtos;
public class GetExecutionProposalsWithPaginationRequest : GetEntitiesWithPaginationRequest
{
    public int? ReceiverId { get; set; }

    public int? SenderId { get; set; }
}
