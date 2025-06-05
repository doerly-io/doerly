import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {BaseApiResponse} from 'app/@core/models/base-api-response';
import {environment} from 'environments/environment.development';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { ProfileResponse } from '../models/responses/ProfileResponse';
import { ProfileRequest } from '../models/requests/ProfileRequest';
import { LanguageDto } from '../models/responses/LanguageDto';
import { LanguageProficiencyDto } from '../models/responses/LanguageProficiencyDto';
import { PageDto } from 'app/@core/models/page.dto';
import { map } from 'rxjs/operators';
import { LanguagesQueryDto } from 'app/modules/profile/models/requests/LanguagesQuery';
import { CompetenceSaveRequest } from 'app/modules/profile/models/requests/CompetenceSaveRequest';

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

  update(model: ProfileRequest): Observable<BaseApiResponse<ProfileResponse>> {
    return this
      .httpClient
      .put<BaseApiResponse<ProfileResponse>>(
        `${this.baseUrl}`,
        model,
        {withCredentials: true}
      )
  }

  uploadImage(image: File): Observable<BaseApiResponse<any>> {
    const formData = new FormData();
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    formData.append('imageFile', image);
    return this
      .httpClient
      .post<BaseApiResponse<any>>(
        `${this.baseUrl}/${userId}/image`,
        formData,
        {withCredentials: true}
      )
  }

  deleteImage(): Observable<BaseApiResponse<any>> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .delete<BaseApiResponse<any>>(
        `${this.baseUrl}/${userId}/image`,
        {withCredentials: true}
      )
  }

  uploadCV(cv: File): Observable<BaseApiResponse<any>> {
    const formData = new FormData();
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    formData.append('cvFile', cv);
    return this
      .httpClient
      .post<BaseApiResponse<any>>(
        `${this.baseUrl}/${userId}/cv`,
        formData,
        {withCredentials: true}
      )
  }

  deleteCV(): Observable<BaseApiResponse<any>> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .delete<BaseApiResponse<any>>(
        `${this.baseUrl}/${userId}/cv`,
        {withCredentials: true}
      )
  }

  getAvailableLanguages(query: LanguagesQueryDto): Observable<PageDto<LanguageDto>> {
    return this
      .httpClient
      .post<BaseApiResponse<PageDto<LanguageDto>>>(
        `${this.baseUrl}/languages/list`,
        query,
        {withCredentials: true}
      )
      .pipe(
        map(response => response.value!)
      );
  }

  addLanguageProficiency(dto: { languageId: number; level: number }): Observable<LanguageProficiencyDto> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .post<LanguageProficiencyDto>(
        `${this.baseUrl}/${userId}/language-proficiency`,
        dto,
        {withCredentials: true}
      );
  }

  updateLanguageProficiency(id: number, dto: { languageId: number; level: number }): Observable<LanguageProficiencyDto> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .put<LanguageProficiencyDto>(
        `${this.baseUrl}/${userId}/language-proficiency/${id}`,
        dto,
        {withCredentials: true}
      );
  }

  deleteLanguageProficiency(id: number): Observable<void> {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    return this
      .httpClient
      .delete<void>(
        `${this.baseUrl}/${userId}/language-proficiency/${id}`,
        {withCredentials: true}
      );
  }

  addCompetence(id: number, model: CompetenceSaveRequest): Observable<BaseApiResponse<any>> {
    return this.httpClient.post<BaseApiResponse<any>>(
      `${this.baseUrl}/${id}/competence`,
      model,
      {withCredentials: true}
    );
  };

  deleteCompetence(id: number, competenceId: number): Observable<BaseApiResponse<any>> {
    return this.httpClient.delete<BaseApiResponse<any>>(
      `${this.baseUrl}/${id}/competence/${competenceId}`,
      { withCredentials: true }
    );
  }

  loadById(userId: number): Observable<BaseApiResponse<ProfileResponse>> {
    return this.httpClient.get<BaseApiResponse<ProfileResponse>>(`${this.baseUrl}/${userId}`, { withCredentials: true });
  }
}
