import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {LoginResponse} from '../models/responses/login.response';
import {environment} from '../../../../environments/environment.development';
import {LoginRequest} from '../models/requests/login-request';
import {RegisterRequest} from '../models/requests/register-request';
import {PasswordResetRequest} from '../models/requests/password-reset-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseApiUrl + '/auth'


  constructor(private readonly httpClient: HttpClient) {
  }

  login(model: LoginRequest): Observable<BaseApiResponse<LoginResponse>> {
    return this.httpClient.post<BaseApiResponse<LoginResponse>>(`${this.baseUrl}/login`, model, {withCredentials: true})
  }

  register(model: RegisterRequest): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/register`, model)
  }

  refreshToken(): Observable<BaseApiResponse<LoginResponse>> {
    return this.httpClient.get<BaseApiResponse<LoginResponse>>(`${this.baseUrl}/refresh`, {withCredentials: true});
  }

  logout(): Observable<BaseApiResponse> {
    return this.httpClient.post<BaseApiResponse>(`${this.baseUrl}/logout`, null, {withCredentials: true});
  }

  requestPasswordRequest(email: string): Observable<BaseApiResponse> {
    return this.httpClient.get<BaseApiResponse>(`${this.baseUrl}/password-reset/${email}`)
  }

  resetPassword(request: PasswordResetRequest) : Observable<BaseApiResponse> {
    return this.httpClient.post<BaseApiResponse>(`${this.baseUrl}/password-reset`, request)
  }

}
