import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { IService, IServiceResponse, ICreateServiceRequest, IUpdateServiceRequest, IPaginatedServiceResponse } from '../models/service.model';
import { IFilterResponse } from '../models/filter.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) {}

  getServicesByUserId(userId: number): Observable<IServiceResponse> {
    console.log('Fetching services for user:', userId);
    return this.http.get<IServiceResponse>(`${this.baseUrl}/service/user/${userId}`);
  }

  createService(request: ICreateServiceRequest): Observable<IServiceResponse> {
    return this.http.post<IServiceResponse>(`${this.baseUrl}/service`, request);
  }

  updateService(request: IUpdateServiceRequest): Observable<IServiceResponse> {
    return this.http.put<IServiceResponse>(`${this.baseUrl}/service/${request.id}`, request);
  }

  deleteService(id: number): Observable<IServiceResponse> {
    return this.http.delete<IServiceResponse>(`${this.baseUrl}/service/${id}`);
  }

  getFiltersByCategoryId(categoryId: number): Observable<IFilterResponse> {
    return this.http.get<IFilterResponse>(`${this.baseUrl}/catalog/filter/${categoryId}`);
  }

  getServicesWithPagination(params: {
    pageNumber: number;
    pageSize: number;
    categoryId?: number;
    filterValues?: { [key: number]: string };
    sortBy?: string;
  }): Observable<IPaginatedServiceResponse> {
    const requestBody = {
      pageInfo: {
        number: params.pageNumber,
        size: params.pageSize
      },
      categoryId: params.categoryId,
      filterValues: params.filterValues || {},
      sortBy: params.sortBy
    };

    return this.http.post<IPaginatedServiceResponse>(`${this.baseUrl}/service/paginated`, requestBody);
  }
} 
