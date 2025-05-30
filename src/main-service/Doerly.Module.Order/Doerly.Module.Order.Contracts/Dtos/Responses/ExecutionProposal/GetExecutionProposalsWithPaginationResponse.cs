namespace Doerly.Module.Order.Contracts.Dtos;
public class GetExecutionProposalsWithPaginationResponse
{
    public int Total { get; set; }

    public List<GetExecutionProposalResponse> ExecutionProposals { get; set; } = new List<GetExecutionProposalResponse>();
}
