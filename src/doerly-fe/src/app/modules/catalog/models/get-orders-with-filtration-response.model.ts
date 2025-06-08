import { ProfileInfoModel } from "./profile-info.model";

export interface GetOrdersWithFiltrationResponse {
    orderId: number;
    categoryId: number;
    name: string;
    price: number;
    dueDate: Date;
    customer: ProfileInfoModel
    createdDate: Date;
}