import { GetOrderResponse } from "./get-order-response";

export interface GetOrdersResponse { 
    total: number;
    orders: GetOrderResponse[];
}