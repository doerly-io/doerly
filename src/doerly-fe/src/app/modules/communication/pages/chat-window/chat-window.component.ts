import {
  Component,
  effect,
  ElementRef,
  inject,
  input,
  model,
  signal,
  ViewChild,
  AfterViewChecked,
  computed
} from '@angular/core';
import { DatePipe, NgClass, NgForOf, NgIf } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CommunicationService } from '../../domain/communication.service';
import { JwtTokenHelper } from '../../../../@core/helpers/jwtToken.helper';
import { ConversationResponse } from '../../models/conversation-response.model';
import { MessageResponse } from '../../models/message-response.model';
import { CommunicationSignalRService } from '../../domain/communication-signalr.service';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { SendMessageRequest } from '../../models/requests/send-message-request.model';
import { Avatar } from 'primeng/avatar';
import {TranslatePipe, TranslateService} from '@ngx-translate/core';
import {MessageType} from '../../models/enums/messageType';
import {MessageStatus} from '../../models/enums/message.status.enum';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [NgIf, NgForOf, DatePipe, ProgressSpinnerModule, ToastModule, NgClass, ButtonDirective, InputText, Avatar, TranslatePipe],
  providers: [MessageService],
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss'],
})
export class ChatWindowComponent {
  private readonly communicationService = inject(CommunicationService);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private readonly messageService = inject(MessageService);
  private readonly communicationSignalR = inject(CommunicationSignalRService);
  private readonly translateService = inject(TranslateService);

  private isFirstLoad = true;
  private typingTimeout: any;

  private pendingReadMessages = new Set<number>();
  private lastUnreadCount = 0;

  private pendingDeliveredMessages = new Set<number>();
  private lastUndeliveredCount = 0;

  private userId = this.jwtTokenHelper.getUserInfo()?.id;
  protected readonly userProfile = computed(() => {
    const conv = this.conversation();
    if (!conv) return null;
    return conv.recipient.id === this.userId ? conv.initiator : conv.recipient;
  });

  protected recipientInfo = computed(() => {
    const conversation = this.conversation();
    if (!conversation) {
      return {
        id: null,
        name: this.translateService.instant('communication.unknown_user'),
        imageUrl: null,
      };
    }
    const isInitiator = this.userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;
    return {
      id: recipient ? recipient.id : null,
      name: recipient ? `${recipient.firstName} ${recipient.lastName}` : this.translateService.instant('communication.unknown_user'),
      imageUrl: recipient?.imageUrl ?? null,
    };
  });

  protected readonly recipientStatus = computed(() => {
    const statusMap = this.communicationSignalR.userStatusSignal();
    const recipientId = this.recipientInfo().id;
    if (recipientId === null) return false;
    return statusMap.get(recipientId) ?? false;
  });

  @ViewChild('messageContainer') messageContainer!: ElementRef<HTMLDivElement>;
  protected message = model<string>('');
  protected readonly loading = signal(true);
  protected readonly conversation = signal<ConversationResponse | null>(null);
  protected readonly typingUser = signal<string | null>(null);
  public conversationId = input<number>();

  constructor() {
    effect(() => {
      const id = this.conversationId();
      if (id) {
        this.loadConversation(id);
        this.communicationSignalR.joinConversation(id.toString());

        this.communicationSignalR.onMessageReceived((newMessage) => {
          this.conversation.update((conv) => {
            if (conv) {
              const tempMessageIndex = conv.messages?.findIndex(m => m.id === newMessage.id);
              if (tempMessageIndex !== undefined && tempMessageIndex >= 0) {
                const updatedMessages = [...(conv.messages || [])];
                updatedMessages[tempMessageIndex] = newMessage;
                return { ...conv, messages: updatedMessages };
              }
              return {
                ...conv,
                messages: [...(conv.messages || []), newMessage],
              };
            }
            return conv;
          });
          this.scrollToBottom();

          if (newMessage.senderId !== this.userId) {
            this.pendingDeliveredMessages.add(newMessage.id);
            this.markMessagesAsDelivered();
          }
        });

        this.communicationSignalR.onMessageStatusChanged((messageId, status) => {
          this.conversation.update((conv) => {
            if (!conv?.messages) return conv;
            const messageIndex = conv.messages.findIndex(m => m.id === messageId);
            if (messageIndex === -1) return conv;

            const updatedMessages = [...conv.messages];
            updatedMessages[messageIndex] = {
              ...updatedMessages[messageIndex],
              status
            };
            return { ...conv, messages: updatedMessages };
          });
          if (status === MessageStatus.READ) {
            this.pendingReadMessages.add(messageId);
          }
        });

        this.communicationSignalR.onUserTyping((fullName) => {
          this.typingUser.set(fullName);
          clearTimeout(this.typingTimeout);
          this.typingTimeout = setTimeout(() => {
            this.typingUser.set(null);
            this.scrollToBottom();
          }, 2000);
          this.scrollToBottom();
        });
      } else {
        this.conversation.set(null);
        this.loading.set(false);
      }
    });

    effect(() => {
      const conv = this.conversation();
      if (conv?.messages?.length) {
        if (this.isFirstLoad) {
          this.scrollToBottom();
          this.isFirstLoad = false;
        }

        const undeliveredCount = conv.messages.filter(
          msg => msg.senderId !== this.userId &&
            msg.status !== MessageStatus.DELIVERED &&
            msg.status !== MessageStatus.READ &&
            !this.pendingDeliveredMessages.has(msg.id)
        ).length;

        if (undeliveredCount !== this.lastUndeliveredCount && undeliveredCount > 0) {
          this.markMessagesAsDelivered();
          this.lastUndeliveredCount = undeliveredCount;
        }

        const unreadCount = conv.messages.filter(
          msg => msg.senderId !== this.userId &&
            msg.status !== MessageStatus.READ &&
            !this.pendingReadMessages.has(msg.id)
        ).length;

        if (unreadCount !== this.lastUnreadCount && unreadCount > 0) {
          this.markMessagesAsRead();
          this.lastUnreadCount = unreadCount;
        }
      }
    });
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      if (this.messageContainer && this.messageContainer.nativeElement) {
        const element = this.messageContainer.nativeElement;
        element.scrollTop = element.scrollHeight;
      }
    });
  }

  private loadConversation(conversationId: number): void {
    this.loading.set(true);
    this.communicationService.getUserConversationById(conversationId).subscribe({
      next: (response) => {
        this.conversation.update(() => response.value ?? null);
        this.loading.set(false);
        if (response.value?.messages?.length) {
          this.scrollToBottom();
        }
      },
      error: () => {
        this.loading.set(false);
        this.messageService.add({
          severity: 'error',
          detail: this.translateService.instant('communication.errors.load_conversation'),
        });
      },
    });
  }

  private markMessagesAsRead(): void {
      const conv = this.conversation();
      if (!conv?.messages) return;

    const unreadMessages = conv.messages.filter(
      msg => msg.senderId !== this.userId &&
        msg.status !== MessageStatus.READ &&
        !this.pendingReadMessages.has(msg.id)
    );

    if (unreadMessages.length === 0) return;

    const messageIds = unreadMessages.map(msg => msg.id);
    messageIds.forEach(id => this.pendingReadMessages.add(id));
    const senderId = unreadMessages[0].senderId.toString();

    this.communicationSignalR.markMessagesAsRead(messageIds, senderId)
      .then(() => {
        messageIds.forEach(id => this.pendingReadMessages.delete(id));
      })
      .catch(err => {
        console.error('Error marking messages as read:', err);
        messageIds.forEach(id => this.pendingReadMessages.delete(id));
      });
  }

  private markMessagesAsDelivered(): void {
    const conv = this.conversation();
    if (!conv?.messages) return;

    const undeliveredMessages = conv.messages.filter(
      msg => msg.senderId !== this.userId &&
        msg.status !== MessageStatus.DELIVERED &&
        msg.status !== MessageStatus.READ &&
        !this.pendingDeliveredMessages.has(msg.id)
    );

    if (undeliveredMessages.length === 0) return;

    const messageIds = undeliveredMessages.map(msg => msg.id);
    messageIds.forEach(id => this.pendingDeliveredMessages.add(id));
    const senderId = undeliveredMessages[0].senderId.toString();

    this.communicationSignalR.markMessagesAsDelivered(messageIds, senderId)
      .then(() => {
        messageIds.forEach(id => this.pendingDeliveredMessages.delete(id));
      })
      .catch(err => {
        console.error('Error marking messages as delivered:', err);
        messageIds.forEach(id => this.pendingDeliveredMessages.delete(id));
      });
  }

  protected isOwnMessage(message: MessageResponse): boolean {
    return message.senderId === this.jwtTokenHelper.getUserInfo()?.id;
  }

  protected trackByMessageId(_: number, message: MessageResponse): number {
    return message.id;
  }

  protected updateMessageText(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.message.set(target.value);

    const conversationId = this.conversationId();
    if (conversationId && target.value.trim()) {
      const userProfile = this.userProfile();
      const fullName = userProfile ? `${userProfile.firstName} ${userProfile.lastName}` : this.translateService.instant('communication.unknown_user');
      this.communicationSignalR.sendTyping(conversationId, fullName)
        .catch(err => console.error('Error sending typing event:', err));
    }
  }

  protected sendMessage(): void {
    const messageContent = this.message()?.trim();
    const conversationId = this.conversationId()!;
    const senderId = this.userId;

    if (!messageContent || !conversationId || !senderId) {
      this.messageService.add({
        severity: 'warn',
        detail: this.translateService.instant('communication.send_message_before_sending'),
      });
      return;
    }

    this.message.set('');

    const sendMessageRequest: SendMessageRequest = {
      conversationId: conversationId,
      senderId: senderId!,
      messageContent
    };
    this.communicationSignalR
      .sendMessage(sendMessageRequest)
      .then(() => {
        setTimeout(() => this.scrollToBottom(), 100);
        console.log('Message sent successfully');
      })
      .catch((err) => {
        this.messageService.add({
          severity: 'error',
          detail: this.translateService.instant('communication.errors.send_message'),
        });
      });
  }

  protected onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      this.messageService.add({
        severity: 'warn',
        detail: 'Файл не вибрано',
      });
      return;
    }

    const conversationId = this.conversationId();
    if (!conversationId) {
      this.messageService.add({
        severity: 'error',
        detail: 'Розмова не обрана',
      });
      return;
    }

    this.communicationService.uploadFile(conversationId, file).subscribe({
      next: (response) => {
        console.log('File uploaded successfully:', response);
        this.messageService.add({
          severity: 'success',
          summary: 'Успіх',
          detail: 'Файл успішно відправлено',
        });
      },
      error: (err) => {
        console.error('Error uploading file:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Помилка',
          detail: 'Не вдалося відправити файл',
        });
      },
    });

    input.value = '';
  }

  protected readonly MessageType = MessageType;
  protected readonly MessageStatus = MessageStatus;
}
