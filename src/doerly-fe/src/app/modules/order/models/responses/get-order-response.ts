import { EOrderStatus } from "../../domain/enums/order-status";
import { EPaymentKind } from "../../domain/enums/payment-kind";

export interface GetOrderResponse { 
    id: number;
    categoryId: number;
    name: string;
    description: string;
    price: number;
    paymentKind: EPaymentKind;
    dueDate: Date;
    status: EOrderStatus;
    customerId: number;
    customerCompletionConfirmation: boolean;
    executorId?: number;
    executorCompletionConfirmation: boolean;
    executionDate?: Date;
    billId?: number;
}