import { EPaymentKind } from "../../domain/enums/payment-kind";

export interface UpdateOrderRequest {
    name: string;
    description: string;
    price: number;
    isPriceNegotiable: boolean;
    useProfileAddress: boolean;
    regionId: number;
    cityId: number;
    paymentKind: EPaymentKind;
    dueDate: Date;
    existingFileUrls?: string[];
}