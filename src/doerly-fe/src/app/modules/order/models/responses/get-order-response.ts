import { EOrderStatus } from "../../domain/enums/order-status";
import { EPaymentKind } from "../../domain/enums/payment-kind";
import { AddressInfoModel } from "./address-info-model";
import { FileInfoModel } from "./file-info-model";
import { ProfileInfoModel } from "./profile-info-model";
import {OrderFeedbackResponse} from 'app/modules/order/models/responses/feedback/order-feedback-response';

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
    useProfileAddress: boolean;
    addressInfo: AddressInfoModel;
    customerId: number;
    customer: ProfileInfoModel;
    customerCompletionConfirmation: boolean;
    executorId?: number;
    executor?: ProfileInfoModel;
    executorCompletionConfirmation: boolean;
    executionDate?: Date;
    billId?: number;
    existingFiles?: FileInfoModel[];
    createdDate: Date;
    feedback: OrderFeedbackResponse;
}
