import { EExecutionProposalStatus } from "../../domain/enums/execution-proposal-status";

export interface ResolveExecutionProposalRequest {
    status: EExecutionProposalStatus;
}