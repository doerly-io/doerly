import { IPageInfo } from 'api/IPageInfo';
import { IPageResponse } from 'api/IPageResponse';
import { IProfileInfo } from 'api/profiles/model';

export interface GetOrdersWithPaginationByPredicatesRequest {
  pageInfo: IPageInfo;
  customerId?: number | null;
  executorId?: number | null;
}

export interface IOrdersPagingResponse extends IPageResponse {
  orders: IOrder[];
}

export enum EPaymentKind {
  Online = 1,
  Cash = 2
}

export enum EOrderStatus {
  Placed = 1,
  InProgress = 2,
  AwaitingPayment = 3,
  AwaitingConfirmation = 4,
  Completed = 5,
  Canceled = 6
}

export interface IOrder {
  id: number;
  categoryId: number;
  name: string;
  description: string;
  price: number;
  isPriceNegotiable: boolean;
  paymentKind: EPaymentKind;
  dueDate: string; // ISO date string
  status: EOrderStatus;
  customerId: number;
  customer: IProfileInfo;
  customerCompletionConfirmed: boolean;
  executorId?: number | null;
  executor?: IProfileInfo | null;
  executorCompletionConfirmed: boolean;
  executionDate?: string | null; // ISO date string
  billId?: number | null;
}
