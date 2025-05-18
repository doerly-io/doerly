import { PageInfo } from "../../../../@core/models/page-info";

export interface GetOrdersWithPaginationByPredicatesRequest { 
    pageInfo: PageInfo;
    customerId?: number | null;
    executorId?: number | null;
}