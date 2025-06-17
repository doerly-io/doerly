export interface SendExecutionProposalRequest {
    orderId: number;
    receiverId: number;
    comment: string | null;
}