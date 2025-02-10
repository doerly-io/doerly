using Doerly.Module.Order.DataAccess.Enums;

namespace Doerly.Module.Order.Domain.Dtos.Requests.ExecutionProposal;
public class ResolveExecutionProposalRequest
{
    public int Id { get; set; }
    public ExecutionProposalStatus ExecutionProposalStatus { get; set; }
}
