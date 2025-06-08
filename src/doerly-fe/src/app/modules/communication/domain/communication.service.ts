import {inject, Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment.development';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {GetConversationResponse} from '../models/responses/get-conversation-response.model';
import {PageInfo} from '../../../@core/models/page-info';
import {ConversationResponse} from '../models/responses/conversation-response.model';


@Injectable({
  providedIn: 'root'
})
export class CommunicationService {
  private baseUrl = environment.baseApiUrl + '/communication'
  private readonly httpClient = inject(HttpClient);

  getUserConversationById(id: number): Observable<BaseApiResponse<ConversationResponse>> {
    return this.httpClient.get<BaseApiResponse<ConversationResponse>>(
      `${this.baseUrl}/conversations/${id}`,
      {
        withCredentials: true
      }
    );
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

  uploadFile(conversationId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('request.File', file);

    return this.httpClient.post<any>(
      `${this.baseUrl}/conversations/${conversationId}/messages/file/send`,
      formData,
      { withCredentials: true }
    );
  }
}
