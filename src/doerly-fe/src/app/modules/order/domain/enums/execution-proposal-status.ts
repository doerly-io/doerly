export enum EExecutionProposalStatus {
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Revoked = 4,
}

export function getExecutionProposalStatusSeverity(status: EExecutionProposalStatus): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (status) {
        case EExecutionProposalStatus.Pending:
            return 'info';
        case EExecutionProposalStatus.Accepted:
            return 'success';
        case EExecutionProposalStatus.Rejected:
            return 'danger';
        case EExecutionProposalStatus.Revoked:
            return 'warn';
        default:
            return 'info';
    }
}