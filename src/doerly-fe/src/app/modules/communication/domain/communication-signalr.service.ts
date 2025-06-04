import {inject, Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';
import {HttpTransportType} from '@microsoft/signalr';
import {SendMessageRequest} from '../models/requests/send-message-request.model';
import {MessageResponse} from '../models/message-response.model';

@Injectable({
  providedIn: 'root'
})
export class CommunicationSignalRService {
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private hubConnection!: signalR.HubConnection;

  // Callbacks for handling events
  private typingCallback?: (fullName: string) => void;
  private messageReceivedCallback?: (message: MessageResponse) => void;

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

    this.hubConnection.on('UserTyping', (fullName: string) => {
      if (this.typingCallback) {
        this.typingCallback(fullName);
      }
    });

    this.hubConnection.on('ReceiveMessage', (message: MessageResponse) => {
      if (this.messageReceivedCallback) {
        this.messageReceivedCallback(message);
      }
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established');
        if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
          this.joinConversation(conversationId.toString());
        } else {
          console.error('Connection not fully established');
        }
      })
      .catch(err => console.log('Error establishing SignalR connection: ' + err));
  }

  public async sendTyping(conversationId: number, fullName: string): Promise<void> {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      return Promise.reject(new Error('SignalR connection is not established'));
    }

    try {
      return await this.hubConnection.invoke('SendTyping', conversationId.toString(), fullName);
    } catch (err) {
      console.error('Error sending typing event:', err);
      throw err;
    }
  }

  public async sendMessage(sendMessageRequest: SendMessageRequest): Promise<void> {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      return Promise.reject(new Error('SignalR connection is not established'));
    }

    try {
      return await this.hubConnection.invoke('SendMessage', {
        conversationId: sendMessageRequest.conversationId,
        senderId: sendMessageRequest.senderId,
        messageContent: sendMessageRequest.messageContent
      });
    } catch (err) {
      console.error('Error sending message:', err);
      throw err;
    }
  }

  public joinConversation(conversationId: string): void {
    this.hubConnection?.invoke('JoinConversation', conversationId).catch(err => console.error(err));
  }

  public onUserTyping(callback: (fullName: string) => void): void {
    this.typingCallback = callback;
  }

  public onMessageReceived(callback: (message: MessageResponse) => void): void {
    this.messageReceivedCallback = callback;
  }
}
