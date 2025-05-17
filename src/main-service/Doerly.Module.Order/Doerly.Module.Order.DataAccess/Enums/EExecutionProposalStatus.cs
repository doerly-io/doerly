namespace Doerly.Module.Order.DataAccess.Enums;

public enum EExecutionProposalStatus : byte
{
    WaitingForApproval = 1,
    Accepted = 2,
    Rejected = 3,
    Revoked = 4
}
