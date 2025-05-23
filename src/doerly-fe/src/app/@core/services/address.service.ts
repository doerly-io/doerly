import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseApiResponse } from '../models/base-api-response';
import { environment } from 'environments/environment.development';
import { Region } from '../models/address/region.model';
import { City } from '../models/address/city.model';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private baseUrl = environment.baseApiUrl + '/address';

  constructor(private readonly httpClient: HttpClient) {}

  getRegions(): Observable<BaseApiResponse<Region[]>> {
    return this.httpClient.get<BaseApiResponse<Region[]>>(`${this.baseUrl}/regions`);
  }

  getCitiesByRegion(regionId: number): Observable<BaseApiResponse<City[]>> {
    return this.httpClient.get<BaseApiResponse<City[]>>(`${this.baseUrl}/regions/${regionId}/cities`);
  }
} 