import { EOrderStatus } from "../../domain/enums/order-status";

export interface UpdateOrderStatusRequest {
    status: EOrderStatus;
    returnUrl?: string | null;
}