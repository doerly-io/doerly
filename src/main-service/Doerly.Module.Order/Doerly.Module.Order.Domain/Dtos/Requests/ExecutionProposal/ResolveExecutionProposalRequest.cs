using Doerly.Module.Order.DataAccess.Enums;

namespace Doerly.Module.Order.Domain.Dtos.Requests;
public class ResolveExecutionProposalRequest
{
    public int Id { get; set; }
    public ExecutionProposalStatus Status { get; set; }
}
