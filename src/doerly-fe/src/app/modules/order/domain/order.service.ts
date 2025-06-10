import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { GetOrdersWithPaginationByPredicatesRequest } from "../models/requests/get-orders-request";
import { BaseApiResponse } from "../../../@core/models/base-api-response";
import { Observable } from "rxjs";
import { GetOrdersResponse } from "../models/responses/get-orders-response";
import { GetOrderResponse } from "../models/responses/get-order-response";
import { CreateOrderRequest } from "../models/requests/create-order-request";
import { UpdateOrderRequest } from "../models/requests/update-order-request";
import { CreateOrderResponse } from "../models/responses/create-order-response";
import { UpdateOrderStatusRequest } from "../models/requests/update-order-status-request";
import { UpdateOrderStatusResponse } from "../models/responses/update-order-status-response";
import { FileInfoModel } from "../models/responses/file-info-model";
import {CreateFeedbackRequest} from 'app/modules/order/models/requests/feedback/create-feedback-request';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = environment.baseApiUrl + '/order/order';

  constructor(private readonly httpClient: HttpClient) { }

  private buildFormData(model: any, files: File[]): FormData {
    const formData = new FormData();
    Object.entries(model).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        if (typeof value === 'boolean') {
          formData.append(key, value ? 'true' : 'false');
        } else if (value instanceof Date) {
          formData.append(key, value.toISOString());
        } else if (typeof value === 'number') {
          formData.append(key, value.toString());
        } else {
          formData.append(key, value as any);
        }
      }
    });
    files.forEach(file => {
      formData.append('files', file);
    });
    return formData;
  }

  getOrdersWithPagination(model: GetOrdersWithPaginationByPredicatesRequest): Observable<BaseApiResponse<GetOrdersResponse>> {
    return this.httpClient.post<BaseApiResponse<GetOrdersResponse>>(`${this.baseUrl}/list`, model);
  }

  getOrder(id: number): Observable<BaseApiResponse<GetOrderResponse>> {
    return this.httpClient.get<BaseApiResponse<GetOrderResponse>>(`${this.baseUrl}/${id}`);
  }

  createOrder(model: CreateOrderRequest, files: File[]): Observable<BaseApiResponse<CreateOrderResponse>> {
    const formData = this.buildFormData(model, files);
    return this.httpClient.post<BaseApiResponse<CreateOrderResponse>>(`${this.baseUrl}`, formData);
  }

  updateOrder(id: number, model: UpdateOrderRequest, newFiles: File[], existingFiles?: FileInfoModel[]): Observable<BaseApiResponse<void>> {
    const formData = this.buildFormData(model, newFiles);
    if (existingFiles && existingFiles.length > 0) {
      existingFiles.forEach(file => {
        formData.append('existingFileNames', file.fileName);
      });
    }
    return this.httpClient.put<BaseApiResponse<void>>(`${this.baseUrl}/${id}`, formData);
  }

  updateOrderStatus(id: number, model: UpdateOrderStatusRequest): Observable<BaseApiResponse<UpdateOrderStatusResponse>> {
    return this.httpClient.put<BaseApiResponse<UpdateOrderStatusResponse>>(`${this.baseUrl}/status/${id}`, model);
  }

  createOrderFeedback(orderId: number, createFeedbackRequest: CreateFeedbackRequest): Observable<BaseApiResponse<void>> {
    return this.httpClient.post<BaseApiResponse<void>>(`${this.baseUrl}/${orderId}/feedback`, createFeedbackRequest);
  }

  updateOrderFeedback(orderId: number, feedbackId: number, createFeedbackRequest: CreateFeedbackRequest): Observable<BaseApiResponse<void>> {
    return this.httpClient.put<BaseApiResponse<void>>(`${this.baseUrl}/${orderId}/feedback/${feedbackId}`, createFeedbackRequest);
  }

  deleteOrderFeedback (orderId: number, feedbackId: number): Observable<BaseApiResponse<void>> {
    return this.httpClient.delete<BaseApiResponse<void>>(`${this.baseUrl}/${orderId}/feedback/${feedbackId}`);
  }
}
