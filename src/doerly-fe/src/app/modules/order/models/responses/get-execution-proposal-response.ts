import { EExecutionProposalStatus } from "../../domain/enums/execution-proposal-status";
import { ProfileInfoModel } from "./profile-info-model";

export interface GetExecutionProposalResponse { 
    id: number;
    orderId: number;
    comment: string | null;
    senderId: number;
    sender: ProfileInfoModel | null;
    receiverId: number;
    receiver: ProfileInfoModel | null;
    status: EExecutionProposalStatus;
}