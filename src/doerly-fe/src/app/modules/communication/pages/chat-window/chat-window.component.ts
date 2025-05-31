import {Component, effect, inject, input, model, signal} from '@angular/core';
import { DatePipe, NgClass, NgForOf, NgIf } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CommunicationService } from '../../domain/communication.service';
import { JwtTokenHelper } from '../../../../@core/helpers/jwtToken.helper';
import { ConversationResponse } from '../../models/conversation-response.model';
import { MessageResponse } from '../../models/message-response.model';
import {CommunicationSignalRService} from '../../domain/communication-signalr.service';
import {ButtonDirective, ButtonIcon, ButtonLabel} from 'primeng/button';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [NgIf, NgForOf, DatePipe, ProgressSpinnerModule, ToastModule, NgClass, ButtonDirective, ButtonLabel, ButtonIcon, InputText],
  providers: [MessageService],
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss'],
})
export class ChatWindowComponent {
  private readonly communicationService = inject(CommunicationService);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private readonly messageService = inject(MessageService);
  private readonly communicationSignalR = inject(CommunicationSignalRService);
  private userId = this.jwtTokenHelper.getUserInfo()?.id;

  protected messageText = model<string>('');

  protected readonly loading = signal(true);
  protected readonly conversation = signal<ConversationResponse | null>(null);

  public conversationId = input<number>();

  constructor() {
    effect(() => {
      const id = this.conversationId();
      if (id) {
        this.loadConversation(id);
        this.communicationSignalR.startConnection(id, this.userId!); //TODO: Fix connections
      } else {
        this.conversation.set(null);
        this.loading.set(false);
      }
    });
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
    this.messageText.set(target.value);
  }

  protected sendMessage(): void {
    const messageContent = this.messageText()?.trim();
    console.log(messageContent);
    // if (!messageContent || !this.conversationId() || !this.userId) {
    //   this.messageService.add({
    //     severity: 'warn',
    //     summary: 'Попередження',
    //     detail: 'Введіть повідомлення перед відправкою',
    //   });
    //   return;
    // }
    //
    // const conversationId = this.conversationId();
    // const senderId = this.userId;
    // const sentAt = new Date().toISOString();
    //
    // // Створюємо тимчасове повідомлення для відображення
    // const tempMessage: MessageResponse = {
    //   id: Date.now(), // Тимчасовий ID
    //   conversationId,
    //   senderId,
    //   messageContent,
    //   sentAt,
    // };
    //
    // // Додаємо тимчасове повідомлення до стану
    // this.conversation.update((conv) => {
    //   if (conv) {
    //     return {
    //       ...conv,
    //       messages: [...(conv.messages || []), tempMessage],
    //     };
    //   }
    //   return conv;
    // });
    //
    // // Очищаємо поле введення
    // this.messageText.set('');
    //
    // // Надсилаємо повідомлення через SignalR
    // this.communicationSignalR
    //   .sendMessage(conversationId, senderId, messageContent)
    //   .then(() => {
    //     console.log('Message sent successfully');
    //   })
    //   .catch((err) => {
    //     this.messageService.add({
    //       severity: 'error',
    //       summary: 'Помилка',
    //       detail: 'Не вдалося відправити повідомлення',
    //     });
    //     // Видаляємо тимчасове повідомлення у разі помилки
    //     this.conversation.update((conv) => {
    //       if (conv) {
    //         return {
    //           ...conv,
    //           messages: conv.messages.filter((m) => m.id !== tempMessage.id),
    //         };
    //       }
    //       return conv;
    //     });
    //   });
  }
}
