import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { IService } from '../models/service.model';
import { IPaginatedServiceResponse } from '../models/service.model';
import { IFilterResponse } from '../models/filter.model';
import { ApiResponse } from '../models/api-response.model';
import { IServiceDetails } from '../models/service-details.model';

interface PaginationRequest {
  pageInfo: {
    number: number;
    size: number;
  };
  categoryId?: number;
  filterValues: any[];
  sortBy?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private apiUrl = `${environment.baseApiUrl}/service`;

  constructor(private http: HttpClient) {}

  getServicesWithPagination(request: PaginationRequest): Observable<IPaginatedServiceResponse> {
    return this.http.post<IPaginatedServiceResponse>(`${this.apiUrl}/paginated`, request);
  }

  getServiceById(id: number): Observable<IService> {
    return this.http.get<IService>(`${this.apiUrl}/${id}`);
  }

  getFiltersByCategoryId(categoryId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/filters/${categoryId}`);
  }

  getServiceDetails(serviceId: number) {
    return this.http.get<ApiResponse<IServiceDetails>>(`${this.apiUrl}/${serviceId}`);
  }
} 