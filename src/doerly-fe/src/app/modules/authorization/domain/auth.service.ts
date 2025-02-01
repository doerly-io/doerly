import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {LoginResponse} from '../models/responses/login.response';
import {environment} from '../../../../environments/environment.development';
import {LoginRequest} from '../models/requests/login-request';
import {RegisterRequest} from '../models/requests/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseApiUrl + '/auth'


  constructor(private readonly httpClient: HttpClient) {
  }

  login(model: LoginRequest): Observable<BaseApiResponse<LoginResponse>> {
    return this.httpClient.post<BaseApiResponse<LoginResponse>>(`${this.baseUrl}/login`, model)
  }

  register(model: RegisterRequest): Observable<BaseApiResponse> {
    return this.httpClient.post<BaseApiResponse>(`${this.baseUrl}/register`, model)
  }

  requestPasswordRequest(email: string): Observable<BaseApiResponse> {
    return this.httpClient.post<BaseApiResponse>(`${this.baseUrl}/request-password-reset`, {email})
  }

}
