import { Component, effect, ElementRef, inject, input, model, signal, ViewChild, AfterViewChecked } from '@angular/core';
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

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [NgIf, NgForOf, DatePipe, ProgressSpinnerModule, ToastModule, NgClass, ButtonDirective, InputText, Avatar],
  providers: [MessageService],
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss'],
})
export class ChatWindowComponent implements AfterViewChecked {
  private readonly communicationService = inject(CommunicationService);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private readonly messageService = inject(MessageService);
  private readonly communicationSignalR = inject(CommunicationSignalRService);
  private userId = this.jwtTokenHelper.getUserInfo()?.id;

  @ViewChild('messageContainer') messageContainer!: ElementRef<HTMLDivElement>;
  protected message = model<string>('');

  protected readonly loading = signal(true);
  protected readonly conversation = signal<ConversationResponse | null>(null);

  public conversationId = input<number>();

  private isFirstLoad = true;

  constructor() {
    effect(() => {
      const id = this.conversationId();
      if (id) {
        this.loadConversation(id);
        this.communicationSignalR.startConnection(id, this.userId!);
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
        });
      } else {
        this.conversation.set(null);
        this.loading.set(false);
      }
    });
  }

  ngAfterViewChecked(): void {
    if (this.conversation() && this.conversation()?.messages?.length && this.isFirstLoad) {
      this.scrollToBottom();
      this.isFirstLoad = false;
    }
  }

  private scrollToBottom(): void {
    if (this.messageContainer && this.messageContainer.nativeElement) {
      this.messageContainer.nativeElement.scrollTop = this.messageContainer.nativeElement.scrollHeight;
    }
  }

  private loadConversation(conversationId: number): void {
    this.loading.set(true);
    this.communicationService.getUserConversationById(conversationId).subscribe({
      next: (response) => {
        this.conversation.update(() => response.value ?? null);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Помилка',
          detail: 'Не вдалося завантажити розмову',
        });
      },
    });
  }

  protected getRecipientName(): string {
    const conversation = this.conversation();
    if (!conversation) return 'Невідомий користувач';

    const isInitiator = this.userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;

    return recipient
      ? `${recipient.firstName} ${recipient.lastName}`
      : 'Невідомий користувач';
  }

  protected getRecipientImageUrl(): string | null {
    const conversation = this.conversation();
    if (!conversation) return null;

    const isInitiator = this.userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;

    return recipient?.imageUrl ?? null;
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
  }

  protected sendMessage(): void {
    const messageContent = this.message()?.trim();
    const conversationId = this.conversationId()!;
    const senderId = this.userId;

    if (!messageContent || !conversationId || !senderId) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Попередження',
        detail: 'Введіть повідомлення перед відправкою',
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
        console.log('Message sent successfully');
      })
      .catch((err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Помилка',
          detail: 'Не вдалося відправити повідомлення',
        });
      });
  }
}
