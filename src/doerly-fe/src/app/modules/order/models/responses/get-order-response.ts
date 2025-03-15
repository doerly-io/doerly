import { OrderStatus } from "../../domain/enums/order-status";
import { PaymentKind } from "../../domain/enums/payment-kind";

export interface GetOrderResponse { 
    id: number;
    categoryId: number;
    name: string;
    description: string;
    price: number;
    paymentKind: PaymentKind;
    dueDate: Date;
    status: OrderStatus;
    customerId: number;
    executorId?: number;
    executionDate?: Date;
    billId?: number;
}