using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Messages;
public record ExecutionProposalStatusChangedMessage(int ExecutionProposalId, EExecutionProposalStatus ExecutionProposalStatus, int ReceiverId);
