import { PageInfo } from "../../../../@core/models/page-info";

export interface GetExecutionProposalsWithPaginationByPredicatesRequest { 
    pageInfo: PageInfo;
    senderId?: number | null;
    receiverId?: number | null;
}