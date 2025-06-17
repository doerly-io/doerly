import { GetExecutionProposalResponse } from "./get-execution-proposal-response";

export interface GetExecutionProposalsResponse {
    total: number;
    executionProposals: GetExecutionProposalResponse[];
}