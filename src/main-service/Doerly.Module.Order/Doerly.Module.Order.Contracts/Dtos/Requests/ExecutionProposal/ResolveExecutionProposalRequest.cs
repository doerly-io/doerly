using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class ResolveExecutionProposalRequest
{
    public int Id { get; set; }
    public EExecutionProposalStatus Status { get; set; }
}
