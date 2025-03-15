import { PageInfo } from "../../../../@core/models/page-info";

export interface GetExecutionProposalsRequest { 
    pageInfo: PageInfo,
    senderId: number | null;
    receiverId: number | null;
}