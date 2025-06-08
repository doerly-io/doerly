import {Component, HostListener, inject, OnDestroy, OnInit, signal} from '@angular/core';
import {ConversationListComponent} from '../conversation-list/conversation-list.component';
import {ChatWindowComponent} from '../chat-window/chat-window.component';
import {ButtonModule} from 'primeng/button';
import {NgIf} from '@angular/common';
import {TranslateModule} from '@ngx-translate/core';
import {MessageResponse} from '../../models/responses/message-response.model';
import {ConversationHeaderResponse} from '../../models/responses/conversation-header-response.model';
import {CommunicationSignalRService} from '../../domain/communication-signalr.service';
import {JwtTokenHelper} from '../../../../@core/helpers/jwtToken.helper';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-communication',
  standalone: true,
  imports: [
    ConversationListComponent,
    ChatWindowComponent,
    ButtonModule,
    NgIf,
    TranslateModule
  ],
  templateUrl: './communication.component.html',
  styleUrl: './communication.component.scss'
})
export class CommunicationComponent implements OnInit, OnDestroy {
  protected readonly selectedConversationId = signal<number | undefined>(undefined);
  protected readonly isMobileView = signal<boolean>(false);
  private readonly MOBILE_BREAKPOINT = 768;

  protected readonly conversations = signal<ConversationHeaderResponse[]>([]);
  private readonly communicationSignalR = inject(CommunicationSignalRService);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private userId = this.jwtTokenHelper.getUserInfo()?.id;

  private readonly route = inject(ActivatedRoute);

  constructor() {
    this.checkScreenSize();
    this.initializeSignalR();
  }

  @HostListener('window:resize')
  onResize() {
    this.checkScreenSize();
  }

  private checkScreenSize() {
    this.isMobileView.set(window.innerWidth <= this.MOBILE_BREAKPOINT);
  }

  private initializeSignalR(): void {
    if (this.userId) {
      this.communicationSignalR.startConnection();

      this.communicationSignalR.onMessageReceived((newMessage: MessageResponse) => {
        this.updateConversations(newMessage);
      });
    }
  }

  private updateConversations(newMessage: MessageResponse): void {
    console.log('Updating conversations with new message:', JSON.stringify(newMessage));
    this.conversations.update(conversations => {
      const updatedConversations = [...conversations];
      const conversationIndex = updatedConversations.findIndex(c => c.id === newMessage.conversationId);
      if (conversationIndex !== -1) {
        updatedConversations[conversationIndex] = {
          ...updatedConversations[conversationIndex],
          lastMessage: newMessage,
        };
      } else {
        updatedConversations.push({
          id: newMessage.conversationId,
          initiator: newMessage.conversation.initiator,
          recipient: newMessage.conversation.recipient,
          lastMessage: newMessage
        });
      }
      return updatedConversations.sort((a, b) =>
        (b.lastMessage?.sentAt?.getTime() ?? 0) - (a.lastMessage?.sentAt?.getTime() ?? 0)
      );
    });
  }

  onConversationSelected(conversationId: number) {
    this.selectedConversationId.set(conversationId);
    this.communicationSignalR.joinConversation(conversationId.toString());
  }

  onBackClick() {
    this.selectedConversationId.set(undefined);
  }

  ngOnInit(): void {
    // Підписуємось на зміни query параметрів
    this.route.queryParams.subscribe(params => {
      const conversationId = params['conversationId'];
      if (conversationId) {
        this.onConversationSelected(Number(conversationId));
      }
    });
  }

  ngOnDestroy(): void {
    this.communicationSignalR.stopConnection().catch(err => console.error('Error during cleanup:', err));
  }
}
