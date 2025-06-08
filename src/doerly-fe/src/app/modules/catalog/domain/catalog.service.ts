import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment.development';
import { IServiceResponse, IPaginatedServiceResponse } from '../models/service.model';
import { IFilterResponse } from '../models/filter.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) {}

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

  getFiltersByCategoryId(categoryId: number): Observable<IFilterResponse> {
    return this.http.get<IFilterResponse>(`${this.baseUrl}/filter/${categoryId}`);
  }
} 