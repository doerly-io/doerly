import { EOrderStatus } from "../../domain/enums/order-status";

export interface UpdateOrderStatusRequest {
    status: EOrderStatus;
    customerId?: number;
    executorId?: number;
    returnUrl?: string | null;
}