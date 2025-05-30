import { EPaymentKind } from "../../domain/enums/payment-kind";

export interface UpdateOrderRequest {
    categoryId: number;
    name: string;
    description: string;
    price: number;
    paymentKind: EPaymentKind;
    dueDate: Date;
}