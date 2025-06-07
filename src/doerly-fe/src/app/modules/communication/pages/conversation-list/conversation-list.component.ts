import {Component, inject, signal, computed, OnInit, output, input, effect} from '@angular/core';
import { NgIf, NgForOf, SlicePipe, DatePipe } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CommunicationService } from '../../domain/communication.service';
import { CommunicationSignalRService } from '../../domain/communication-signalr.service';
import {PageInfo} from '../../../../@core/models/page-info';
import {ConversationHeaderResponse} from '../../models/conversation-header-response.model';
import {JwtTokenHelper} from '../../../../@core/helpers/jwtToken.helper';
import {TranslatePipe, TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-conversation-list',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    SlicePipe,
    DatePipe,
    ProgressSpinnerModule,
    PaginatorModule,
    ToastModule,
    TranslatePipe,
  ],
  providers: [MessageService],
  templateUrl: './conversation-list.component.html',
  styleUrls: ['./conversation-list.component.scss'],
})
export class ConversationListComponent implements OnInit {
  private readonly communicationService = inject(CommunicationService);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);
  private readonly translateService = inject(TranslateService);
  private readonly communicationSignalR = inject(CommunicationSignalRService);

  protected readonly loading = signal(false);
  protected readonly pageSize = signal(10);
  protected readonly currentPage = signal(1);
  protected readonly totalRecords = signal(0);
  protected readonly conversations = signal<ConversationHeaderResponse[]>([]);
  protected readonly currentConversationId = signal<number | null>(null);

  conversationUpdates = input<ConversationHeaderResponse[] | null>(null);

  selectedConversationId = output<number>();

  protected readonly paginationRequest = computed<PageInfo>(() => ({
    number: this.currentPage(),
    size: this.pageSize(),
  }));

  constructor() {
    this.handleConversationUpdates();
    this.handleLastMessageUpdates();
  }

  ngOnInit(): void {
    this.loadConversations();
  }

  protected onPageChange(event: PaginatorState): void {
    this.currentPage.set(event.page ?? 1);
    this.pageSize.set(event.rows ?? 10);
    this.loadConversations();
  }

  private loadConversations(): void {
    this.loading.set(true);

    this.communicationService
      .getUserConversationWithPagination(this.paginationRequest())
      .subscribe({
        next: (response) => {
          this.conversations.set(response.value?.conversations ?? []);
          this.totalRecords.set(response.value?.total ?? 0);
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
        },
      });
  }

  protected getRecipientName(conversation: ConversationHeaderResponse): string {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    const isInitiator = userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;

    if (!recipient) {
      return this.translateService.instant('communication.unknown_user');
    }

    return `${recipient.firstName} ${recipient.lastName}`;
  }

  protected getRecipientImageUrl(conversation: ConversationHeaderResponse): string | null {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    const isInitiator = userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;
    return recipient?.imageUrl ?? null;
  }

  protected trackByConversationId(_: number, conversation: ConversationHeaderResponse): number {
    return conversation.id;
  }

  protected navigateToConversation(conversationId: number): void {
    const previousConversationId = this.currentConversationId();
    if (previousConversationId !== null && previousConversationId !== conversationId) {
      this.communicationSignalR.leaveConversation(previousConversationId.toString());
    }
    this.currentConversationId.set(conversationId);
    this.selectedConversationId.emit(conversationId);
  }

  private handleConversationUpdates(): void {
    effect(() => {
      const updates = this.conversationUpdates();
      if (updates) {
        this.conversations.update(currentConversations => {
          const updatedConversations = [...currentConversations];
          updates.forEach(updatedConv => {
            const index = updatedConversations.findIndex(conv => conv.id === updatedConv.id);
            if (index !== -1) {
              updatedConversations[index] = updatedConv;
            } else {
              updatedConversations.push(updatedConv);
            }
          });
          return updatedConversations;
        });
      }
    });
  }

  private handleLastMessageUpdates(): void {
    this.communicationSignalR.onLastMessageUpdate((message) => {
      this.conversations.update(currentConversations => {
        const updatedConversations = [...currentConversations];
        const index = updatedConversations.findIndex(conv => conv.id === message.conversationId);
        if (index !== -1) {
          updatedConversations[index].lastMessage = message;
          // Sort conversations by last message time
          return updatedConversations.sort((a, b) =>
            (b.lastMessage?.sentAt?.getTime() ?? 0) - (a.lastMessage?.sentAt?.getTime() ?? 0)
          );
        }
        return updatedConversations;
      });
    });
  }
}
