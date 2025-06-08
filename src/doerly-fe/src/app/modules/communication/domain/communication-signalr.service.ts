import {inject, Injectable, signal} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {JwtTokenHelper} from '../../../@core/helpers/jwtToken.helper';
import {HttpTransportType} from '@microsoft/signalr';
import {SendMessageRequest} from '../models/requests/send-message-request.model';
import {MessageResponse} from '../models/responses/message-response.model';
import {environment} from '../../../../environments/environment.development';
import { MessageStatus } from '../models/enums/message.status.enum';
import { ConversationHeaderResponse } from '../models/responses/conversation-header-response.model';

@Injectable({
  providedIn: 'root'
})
export class CommunicationSignalRService {
  private baseUrl = environment.baseApiUrl.replace(/\/api$/, '') + '/communicationhub';

  private readonly jwtTokenHelper = inject(JwtTokenHelper);

  private hubConnection!: signalR.HubConnection;

  private readonly userStatus = signal(new Map<number, boolean>());

  // Callbacks for handling events
  private typingCallback?: (fullName: string) => void;
  private messageReceivedCallback?: (message: MessageResponse) => void;
  private messageStatusCallback?: (messageId: number, status: MessageStatus) => void;
  private lastMessageUpdateCallback?: (message: MessageResponse) => void;

  get userStatusSignal() {
    return this.userStatus;
  }

  private updateUserStatus(userId: number, isOnline: boolean): void {
    const current = this.userStatus();
    const updated = new Map(current);
    updated.set(userId, isOnline);
    this.userStatus.set(updated);
  }

  public startConnection = () => {
    const accessToken = this.jwtTokenHelper.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl, {
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

    this.hubConnection.on('UserStatusChanged', (userId: string, isOnline: boolean) => {
      const parsedUserId = parseInt(userId);
      console.log('User status changed:', parsedUserId, isOnline);
      this.updateUserStatus(parsedUserId, isOnline);
    });

    this.hubConnection.on('MessageDelivered', (messageId: number) => {
      if (this.messageStatusCallback) {
        this.messageStatusCallback(messageId, MessageStatus.DELIVERED);
      }
    });

    this.hubConnection.on('MessageRead', (messageId: number) => {
      if (this.messageStatusCallback) {
        this.messageStatusCallback(messageId, MessageStatus.READ);
      }
    });

    this.hubConnection.on('UpdateLastMessage', (message: MessageResponse) => {
      if (this.lastMessageUpdateCallback) {
        this.lastMessageUpdateCallback(message);
      }
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established');
      })
      .catch(err => console.log('Error establishing SignalR connection: ' + err));
  }

  public async stopConnection(): Promise<void> {
    if (!this.hubConnection) {
      console.warn('No active SignalR connection to stop');
      return;
    }

    try {
      await this.hubConnection.stop();
      console.log('SignalR connection stopped successfully');
      this.hubConnection = null as any;
      this.typingCallback = undefined;
      this.messageReceivedCallback = undefined;
    } catch (err) {
      console.error('Error stopping SignalR connection:', err);
      throw err;
    }
  }

  public async markMessagesAsDelivered(messageIds: number[], senderId: string): Promise<void> {
    if (!this.isConnected()) {
      return Promise.reject(new Error('SignalR connection is not established'));
    }

    try {
      return await this.hubConnection.invoke('MarkMessagesAsDelivered', messageIds, senderId);
    } catch (err) {
      console.error('Error marking messages as delivered:', err);
      throw err;
    }
  }

  public async markMessagesAsRead(messageIds: number[], senderId: string): Promise<void> {
    if (!this.isConnected()) {
      return Promise.reject(new Error('SignalR connection is not established'));
    }

    try {
      return await this.hubConnection.invoke('MarkMessagesAsRead', messageIds, senderId);
    } catch (err) {
      console.error('Error marking messages as read:', err);
      throw err;
    }
  }

  public async sendTyping(conversationId: number, fullName: string): Promise<void> {
    if (!this.isConnected()) {
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
    if (!this.isConnected()) {
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

  public leaveConversation(conversationId: string): void {
    this.hubConnection?.invoke('LeaveConversation', conversationId).catch(err => console.error(err));
  }

  public onUserTyping(callback: (fullName: string) => void): void {
    this.typingCallback = callback;
  }

  public onMessageReceived(callback: (message: MessageResponse) => void): void {
    this.messageReceivedCallback = callback;
  }

  public onMessageStatusChanged(callback: (messageId: number, status: MessageStatus) => void): void {
    this.messageStatusCallback = callback;
  }

  public onLastMessageUpdate(callback: (message: MessageResponse) => void): void {
    this.lastMessageUpdateCallback = callback;
  }

  private isConnected(): boolean {
    return this.hubConnection.state === signalR.HubConnectionState.Connected;
  }
}
