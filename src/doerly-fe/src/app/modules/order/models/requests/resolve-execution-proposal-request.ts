import { ExecutionProposalStatus } from "../../domain/enums/execution-proposal-status";

export interface ResolveExecutionProposalRequest {
    id: number;
    status: ExecutionProposalStatus;
}