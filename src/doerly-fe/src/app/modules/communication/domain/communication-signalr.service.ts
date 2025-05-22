import {inject, Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {environment} from '../../../../environments/environment.development';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';

@Injectable({
  providedIn: 'root'
})
export class CommunicationSignalRService {
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private hubConnection!: signalR.HubConnection;

  public startConnection = (conversationId: number, userId: number) => {
    const accessToken = this.jwtTokenHelper.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseApiUrl}/communicationhub`, {
        accessTokenFactory: () => accessToken,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        withCredentials: true})
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection started');
        this.joinConversation(conversationId, userId);
      })
      .catch(err => console.log('Error establishing SignalR connection: ' + err));
  }

  public joinConversation(conversationId: number, userId: number): void {
    this.hubConnection?.invoke('JoinConversation', conversationId, userId).catch(err => console.error(err));
  }
}
