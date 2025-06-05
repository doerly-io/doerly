import { Injectable } from '@angular/core';
import { environment } from 'environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { ICategory } from 'app/@core/models/category/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private baseUrl = environment.baseApiUrl + '/category';

  constructor(private readonly httpClient: HttpClient) {}

  getCategories(): Observable<BaseApiResponse<ICategory[]>> {
    return this.httpClient.get<BaseApiResponse<ICategory[]>>(`${this.baseUrl}`);
  }
}
