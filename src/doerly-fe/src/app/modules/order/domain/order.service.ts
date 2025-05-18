import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { GetOrdersWithPaginationByPredicatesRequest } from "../models/requests/get-orders-request";
import { BaseApiResponse } from "../../../@core/models/base-api-response";
import { Observable } from "rxjs";
import { GetOrdersResponse } from "../models/responses/get-orders-response";
import { GetOrderResponse } from "../models/responses/get-order-response";
import { CreateOrderRequest } from "../models/requests/create-order-request";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = environment.baseApiUrl + '/order/order';

  constructor(private readonly httpClient: HttpClient) {}

  getOrdersWithPagination(model: GetOrdersWithPaginationByPredicatesRequest): Observable<BaseApiResponse<GetOrdersResponse>> {
    return this.httpClient.post<BaseApiResponse<GetOrdersResponse>>(`${this.baseUrl}/list`, model);
  }

  getOrder(id: number): Observable<BaseApiResponse<GetOrderResponse>> {
    return this.httpClient.get<BaseApiResponse<GetOrderResponse>>(`${this.baseUrl}/${id}`);
  }

  createOrder(model: CreateOrderRequest): Observable<BaseApiResponse<number>> {
    return this.httpClient.post<BaseApiResponse<number>>(`${this.baseUrl}`, model);
  }

  cancelOrder(id: number): Observable<BaseApiResponse<void>> {
    return this.httpClient.delete<BaseApiResponse<void>>(`${this.baseUrl}/${id}`);
  }
}