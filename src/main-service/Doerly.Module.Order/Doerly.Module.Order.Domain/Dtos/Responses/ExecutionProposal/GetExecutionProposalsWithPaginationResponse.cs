namespace Doerly.Module.Order.Domain.Dtos.Responses;
public class GetExecutionProposalsWithPaginationResponse
{
    public int Total { get; set; }

    public List<GetExecutionProposalResponse> ExecutionProposals { get; set; } = new List<GetExecutionProposalResponse>();
}
