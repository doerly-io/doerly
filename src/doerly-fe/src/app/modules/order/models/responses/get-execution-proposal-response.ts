import { EExecutionProposalStatus } from "../../domain/enums/execution-proposal-status";

export interface GetExecutionProposalResponse { 
    id: number;
    orderId: number;
    comment: string | null;
    senderId: number;
    receiverId: number;
    status: EExecutionProposalStatus;
}