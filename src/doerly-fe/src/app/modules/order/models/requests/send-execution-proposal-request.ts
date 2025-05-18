export interface SendExecutionProposalRequest {
    orderId: number;
    senderId: number;
    receiverId: number;
    comment: string | null;
}