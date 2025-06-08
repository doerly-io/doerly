import { ProfileInfoModel } from "./profile-info.model";

export interface GetOrdersWithFiltrationResponse {
    count: number;
    items: OrderModel[];
}

export interface OrderModel {
    orderId: number;
    categoryId: number;
    name: string;
    price: number;
    dueDate: Date;
    customer: ProfileInfoModel
    createdDate: Date;
}