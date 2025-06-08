import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment.development';
import { IService, ICreateServiceRequest, IUpdateServiceRequest, IServiceResponse } from '../models/service.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private baseUrl = environment.baseApiUrl;
  private readonly http = inject(HttpClient);

  createService(service: ICreateServiceRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/service`, service);
  }

  updateService(id: number, service: IUpdateServiceRequest): Observable<any> {
    return this.http.put(`${this.baseUrl}/service/${id}`, service);
  }

  getServicesByUserId(userId: number): Observable<IServiceResponse> {
    return this.http.get<IServiceResponse>(`${this.baseUrl}/service/user/${userId}`);
  }

  deleteService(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/service/${id}`);
  }
} 