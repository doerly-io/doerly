using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Messages;
public record ExecutionProposalStatucUpdatedMessage(int ExecutionProposalId, EExecutionProposalStatus ExecutionProposalStatus);
