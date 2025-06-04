import { EPaymentKind } from "../../domain/enums/payment-kind";

export interface CreateOrderRequest {
    categoryId: number;
    name: string;
    description: string;
    price: number;
    isPriceNegotiable: boolean;
    paymentKind: EPaymentKind;
    dueDate: Date;
}