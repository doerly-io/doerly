import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BaseApiResponse} from 'app/@core/models/base-api-response';
import {environment} from 'environments/environment.development';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { ProfileResponse } from '../models/responses/ProfileResponse';
import { ProfileRequest } from '../models/requests/ProfileRequest';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private baseUrl = environment.baseApiUrl + '/profile'

  constructor(
    private readonly httpClient: HttpClient,
    private readonly jwtTokenHelper: JwtTokenHelper
  ) {
  }

  load(): Observable<BaseApiResponse<ProfileResponse>> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .get<BaseApiResponse<ProfileResponse>>(
        `${this.baseUrl}/${userId}`,
        {withCredentials: true}
      )
  }

  save(model: ProfileRequest): Observable<BaseApiResponse<ProfileResponse>> {
    return this
      .httpClient
      .put<BaseApiResponse<ProfileResponse>>(
        `${this.baseUrl}`,
        model,
        {withCredentials: true}
      )
  }

}
