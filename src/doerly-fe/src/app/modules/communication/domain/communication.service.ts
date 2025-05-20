import {inject, Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment.development';
import {MessageResponse} from '../models/message-response.model';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {HttpClient} from '@angular/common/http';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';
import {Observable} from 'rxjs';
import {ConversationResponse} from '../models/conversation-response.model';
import {GetConversationResponse} from '../models/responses/get-conversation-response.model';
import {PageInfo} from '../../../@core/models/page-info';


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

  getUserConversationWithPagination(pagination: PageInfo): Observable<BaseApiResponse<GetConversationResponse>> {
    const params = {
      pageNumber: pagination.number,
      pageSize: pagination.size
    };

    return this.httpClient.get<BaseApiResponse<GetConversationResponse>>(
      `${this.baseUrl}/conversations`,
      {
        params,
        withCredentials: true
      }
    );
  }
}
