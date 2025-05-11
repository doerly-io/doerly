import {inject, Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment.development';
import {MessageResponse} from '../models/responses/message-response.model';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {HttpClient} from '@angular/common/http';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';
import {Observable} from 'rxjs';
import {ProfileResponse} from '../../profile/models/responses/ProfileResponse';
import {ConversationResponse} from '../models/responses/conversation-response.model';


@Injectable({
  providedIn: 'root'
})
export class CommunicationService {
  private baseUrl = environment.baseApiUrl + '/communication'
  private readonly httpClient = inject(HttpClient);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);

  getMessageById(id: number): Observable<BaseApiResponse<MessageResponse>> {
    return this
      .httpClient
      .get<BaseApiResponse<MessageResponse>>(
        `${this.baseUrl}/messages/${id}`,
        {withCredentials: true}
      )
  }

  getConversations(): Observable<BaseApiResponse<ConversationResponse[]>> {
    return this
      .httpClient
      .get<BaseApiResponse<MessageResponse[]>>(
        `${this.baseUrl}/conversations`,
        {withCredentials: true}
      )
  }
}
