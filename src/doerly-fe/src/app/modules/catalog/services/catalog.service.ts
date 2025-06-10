import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment.development';
import { IService, ICreateServiceRequest, IUpdateServiceRequest } from '../models/service.model';
import { IFilterResponse } from '../models/filter.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.baseApiUrl;

  getServicesByUserId(userId: number): Observable<{ value: IService[], isSuccess: boolean }> {
    return this.http.get<{ value: IService[], isSuccess: boolean }>(`${this.baseUrl}/service/user/${userId}`);
  }

  createService(request: ICreateServiceRequest): Observable<{ value: IService, isSuccess: boolean }> {
    return this.http.post<{ value: IService, isSuccess: boolean }>(`${this.baseUrl}/service`, request);
  }

  updateService(id: number, request: IUpdateServiceRequest): Observable<{ value: IService, isSuccess: boolean }> {
    return this.http.put<{ value: IService, isSuccess: boolean }>(`${this.baseUrl}/service/${id}`, request);
  }

  deleteService(id: number): Observable<{ isSuccess: boolean }> {
    return this.http.delete<{ isSuccess: boolean }>(`${this.baseUrl}/service/${id}`);
  }

  getFiltersByCategoryId(categoryId: number): Observable<IFilterResponse> {
    return this.http.get<IFilterResponse>(`${this.baseUrl}/catalog/filter/${categoryId}`);
  }
} 