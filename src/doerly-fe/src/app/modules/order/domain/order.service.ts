import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment.development";
import { HttpClient, HttpParams } from "@angular/common/http";
import { GetOrdersWithPaginationByPredicatesRequest } from "../models/requests/get-orders-request";
import { BaseApiResponse } from "../../../@core/models/base-api-response";
import { Observable } from "rxjs";
import { GetOrdersResponse } from "../models/responses/get-orders-response";
import { GetOrderResponse } from "../models/responses/get-order-response";
import { setPaginationParams } from "../../../@core/helpers/pagination.helper";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = environment.baseApiUrl + '/order'

  constructor(private readonly httpClient: HttpClient) {}

  getOrdersWithPagination(model: GetOrdersWithPaginationByPredicatesRequest): Observable<BaseApiResponse<GetOrdersResponse>> {
    // const params = setPaginationParams(model.pageInfo);

    return this.httpClient.post<BaseApiResponse<GetOrdersResponse>>(`${this.baseUrl}/list`, model);
  }

  getOrder(id: number): Observable<BaseApiResponse<GetOrderResponse>> {
    return this.httpClient.get<BaseApiResponse<GetOrderResponse>>(`${this.baseUrl}/${id}`);
  }
}