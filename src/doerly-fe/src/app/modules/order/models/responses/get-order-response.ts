import { EOrderStatus } from "../../domain/enums/order-status";
import { EPaymentKind } from "../../domain/enums/payment-kind";
import { ProfileInfoModel } from "./profile-info-model";

export interface GetOrderResponse { 
    id: number;
    categoryId: number;
    name: string;
    description: string;
    price: number;
    isPriceNegotiable: boolean;
    paymentKind: EPaymentKind;
    dueDate: Date;
    status: EOrderStatus;
    customerId: number;
    customer: ProfileInfoModel;
    customerCompletionConfirmation: boolean;
    executorId?: number;
    executor?: ProfileInfoModel;
    executorCompletionConfirmation: boolean;
    executionDate?: Date;
    billId?: number;
}