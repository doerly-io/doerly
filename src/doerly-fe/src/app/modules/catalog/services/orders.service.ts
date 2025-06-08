import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment.development';
import { IService, ICreateServiceRequest, IUpdateServiceRequest, IServiceResponse } from '../models/service.model';
import { GetOrdersWithFiltrationRequest } from '../models/get-orders-with-filtration-request.model';
import { GetOrdersWithFiltrationResponse } from '../models/get-orders-with-filtration-response.model';
import { BaseApiResponse } from 'app/@core/models/base-api-response';

@Injectable({
    providedIn: 'root'
})
export class OrdersService {
    private baseUrl = environment.baseApiUrl + '/catalog/orders';
    private readonly http = inject(HttpClient);

    getOrdersWithPagination(model: GetOrdersWithFiltrationRequest): Observable<BaseApiResponse<GetOrdersWithFiltrationResponse>> {
        let params = new HttpParams();

        // Flatten pageInfo
        if (model.pageInfo) {
            params = params.set('pageInfo.number', model.pageInfo.number.toString());
            params = params.set('pageInfo.size', model.pageInfo.size.toString());
        }

        Object.entries(model).forEach(([key, value]) => {
            if (value !== undefined && value !== null) {
                params = params.set(key, value as string);
            }
        });
        return this.http.get<BaseApiResponse<GetOrdersWithFiltrationResponse>>(
            `${this.baseUrl}/get-orders-with-pagination`,
            { params }
        );
    }
} 