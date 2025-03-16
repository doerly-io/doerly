import { PaymentKind } from "../../domain/enums/payment-kind";

export interface CreateOrderRequest {
    categoryId: number;
    name: string;
    description: string;
    price: number;
    paymentKind: PaymentKind;
    dueDate: Date;
    customerId: number;
}