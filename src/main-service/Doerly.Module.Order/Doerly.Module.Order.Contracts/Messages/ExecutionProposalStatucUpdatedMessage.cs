using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Messages;
public record ExecutionProposalStatucUpdatedMessage(int ExecutionProposalId, EExecutionProposalStatus ExecutionProposalStatus);
