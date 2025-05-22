import {inject, Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {environment} from '../../../../environments/environment.development';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';
import {HttpTransportType} from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class CommunicationSignalRService {
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private hubConnection!: signalR.HubConnection;

  public startConnection = (conversationId: number, userId: number) => {
    const accessToken = this.jwtTokenHelper.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5051/communicationhub`, {
        accessTokenFactory: () => accessToken,
        transport: HttpTransportType.WebSockets,
        skipNegotiation: true
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established');
        if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
          this.joinConversation(conversationId, userId);
        } else {
          console.error('Connection not fully established');
        }
      })
      .catch(err => console.log('Error establishing SignalR connection: ' + err));
  }

  public joinConversation(conversationId: number, userId: number): void {
    this.hubConnection?.invoke('JoinConversation', conversationId, userId).catch(err => console.error(err));
  }
}
