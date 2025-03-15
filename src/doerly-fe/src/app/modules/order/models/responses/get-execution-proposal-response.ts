export interface GetExecutionProposalResponse { 
    id: number;
    orderId: number;
    comment: string | null;
    senderId: number;
    receiverId: number;
    status: number;
}