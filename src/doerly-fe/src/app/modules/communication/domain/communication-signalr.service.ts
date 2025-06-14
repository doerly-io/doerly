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

  private isConnecting = false;

  private connectionPromise: Promise<void> | null = null;

  private readonly MAX_RETRY_ATTEMPTS = 3;
  private readonly RETRY_DELAY_MS = 1000;

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

  public startConnection = async () => {
    if (this.isConnecting) {
      return this.connectionPromise;
    }

    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      return Promise.resolve();
    }

    this.isConnecting = true;
    const accessToken = this.jwtTokenHelper.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl, {
        accessTokenFactory: () => accessToken,
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.setupHubHandlers();

    this.connectionPromise = this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection established');
        this.isConnecting = false;
      })
      .catch(err => {
        console.error('Error establishing SignalR connection:', err);
        this.isConnecting = false;
        this.connectionPromise = null;
        throw err;
      });

    return this.connectionPromise;
  }

  private setupHubHandlers(): void {
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
  }

  public async stopConnection(): Promise<void> {
    if (!this.hubConnection) {
      console.warn('No active SignalR connection to stop');
      return;
    }

    try {
      // Remove all handlers before stopping
      this.hubConnection.off('UserTyping');
      this.hubConnection.off('ReceiveMessage');
      this.hubConnection.off('UserStatusChanged');
      this.hubConnection.off('MessageDelivered');
      this.hubConnection.off('MessageRead');
      this.hubConnection.off('UpdateLastMessage');

      // Clear callbacks
      this.typingCallback = undefined;
      this.messageReceivedCallback = undefined;
      this.messageStatusCallback = undefined;
      this.lastMessageUpdateCallback = undefined;

      await this.hubConnection.stop();
      console.log('SignalR connection stopped successfully');
      this.hubConnection = null as any;
      this.connectionPromise = null;
      this.isConnecting = false;
    } catch (err) {
      console.error('Error stopping SignalR connection:', err);
      throw err;
    }
  }

  private async ensureConnection(): Promise<void> {
    if (this.isConnected()) {
      return;
    }

    let attempts = 0;
    while (attempts < this.MAX_RETRY_ATTEMPTS) {
      try {
        await this.startConnection();
        if (this.isConnected()) {
          return;
        }
      } catch (error) {
        console.warn(`Connection attempt ${attempts + 1} failed:`, error);
      }

      attempts++;
      if (attempts < this.MAX_RETRY_ATTEMPTS) {
        await new Promise(resolve => setTimeout(resolve, this.RETRY_DELAY_MS));
      }
    }

    throw new Error('Failed to establish SignalR connection after multiple attempts');
  }

  private async invokeWithConnectionCheck(methodName: string, ...args: any[]): Promise<void> {
    try {
      await this.ensureConnection();
      await this.hubConnection!.invoke(methodName, ...args);
    } catch (error) {
      console.error(`Error invoking ${methodName}:`, error);
      throw error;
    }
  }

  public async markMessagesAsDelivered(messageIds: number[], senderId: string): Promise<void> {
    return this.invokeWithConnectionCheck('MarkMessagesAsDelivered', messageIds, senderId);
  }

  public async markMessagesAsRead(messageIds: number[], senderId: string): Promise<void> {
    return this.invokeWithConnectionCheck('MarkMessagesAsRead', messageIds, senderId);
  }

  public async sendTyping(conversationId: number, fullName: string): Promise<void> {
    return this.invokeWithConnectionCheck('SendTyping', conversationId.toString(), fullName);
  }

  public async sendMessage(sendMessageRequest: SendMessageRequest): Promise<void> {
    return this.invokeWithConnectionCheck('SendMessage', {
      conversationId: sendMessageRequest.conversationId,
      senderId: sendMessageRequest.senderId,
      messageContent: sendMessageRequest.messageContent
    });
  }

  public async joinConversation(conversationId: string): Promise<void> {
    return this.invokeWithConnectionCheck('JoinConversation', conversationId);
  }

  public async leaveConversation(conversationId: string): Promise<void> {
    return this.invokeWithConnectionCheck('LeaveConversation', conversationId);
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
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }
}
