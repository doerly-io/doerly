import {inject, Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment.development';
import {BaseApiResponse} from '../../../@core/models/base-api-response';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {GetConversationResponse} from '../models/responses/get-conversation-response.model';
import {PageInfo} from '../../../@core/models/page-info';
import {ConversationResponse} from '../models/responses/conversation-response.model';
import {CreateConversationRequest} from '../models/requests/create-conversation-request';
import {SendMessageRequest} from '../models/requests/send-message-request';

@Injectable({
  providedIn: 'root'
})
export class CommunicationService {
  private readonly apiUrl = `${environment.baseApiUrl}/communication`;

  constructor(private http: HttpClient) {}

  getConversationWithUser(userId: number): Observable<BaseApiResponse<number | null>> {
    return this.http.get<BaseApiResponse<number | null>>(`${this.apiUrl}/conversations/with-user/${userId}`, {
      withCredentials: true
    });
  }

  createConversation(recipientId: number): Observable<BaseApiResponse<number>> {
    const request: CreateConversationRequest = { recipientId };
    return this.http.post<BaseApiResponse<number>>(
      `${this.apiUrl}/conversations`,
      request,
      { withCredentials: true }
    );
  }

  sendMessage(request: SendMessageRequest): Observable<BaseApiResponse<number>> {
    return this.http.post<BaseApiResponse<number>>(
      `${this.apiUrl}/messages/send`,
      request,
      { withCredentials: true }
    );
  }

  getUserConversationById(id: number): Observable<BaseApiResponse<ConversationResponse>> {
    return this.http.get<BaseApiResponse<ConversationResponse>>(
      `${this.apiUrl}/conversations/${id}`,
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

    return this.http.get<BaseApiResponse<GetConversationResponse>>(
      `${this.apiUrl}/conversations`,
      {
        params,
        withCredentials: true
      }
    );
  }

  uploadFile(conversationId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('imageFile', file);

    return this.http.post<any>(
      `${this.apiUrl}/conversations/${conversationId}/messages/file/send`,
      formData,
      { withCredentials: true }
    );
  }
}
