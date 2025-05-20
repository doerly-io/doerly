import {Component, inject, signal, computed, OnInit} from '@angular/core';
import { NgIf, NgForOf, SlicePipe, DatePipe } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CommunicationService } from '../../domain/communication.service';
import { Router } from '@angular/router';
import {PageInfo} from '../../../../@core/models/page-info';
import {ConversationResponse} from '../../models/conversation-response.model';
import {JwtTokenHelper} from '../../../../@core/helpers/jwtToken.helper';

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
  ],
  providers: [MessageService],
  templateUrl: './conversation-list.component.html',
  styleUrls: ['./conversation-list.component.scss'],
})
export class ConversationListComponent implements OnInit {
  private readonly communicationService = inject(CommunicationService);
  private readonly messageService = inject(MessageService);
  private readonly router = inject(Router);
  private readonly jwtTokenHelper = inject(JwtTokenHelper);

  protected readonly loading = signal(false);
  protected readonly pageSize = signal(10);
  protected readonly currentPage = signal(1);
  protected readonly totalRecords = signal(0);
  protected readonly conversations = signal<ConversationResponse[]>([]);

  protected readonly paginationRequest = computed<PageInfo>(() => ({
    number: this.currentPage(),
    size: this.pageSize(),
  }));

  ngOnInit(): void {
    this.loadConversations();
  }

  protected onPageChange(event: PaginatorState): void {
    this.currentPage.set(event.page ?? 0);
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
        //TODO:   Add toasts
        },
      });
  }

  protected getRecipientName(conversation: ConversationResponse): string {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    const isInitiator = userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;

    if (!recipient) {
      return 'Невідомий користувач'; //TODO: Add toast
    }

    return `${recipient.firstName} ${recipient.lastName}`;
  }

  protected getRecipientImageUrl(conversation: ConversationResponse): string | null {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    const isInitiator = userId === conversation.initiator.id;
    const recipient = isInitiator ? conversation.recipient : conversation.initiator;
    return recipient?.imageUrl ?? null;
  }

  protected trackByConversationId(_: number, conversation: ConversationResponse): number {
    return conversation.id;
  }

  protected navigateToConversation(conversationId: number): void {
    this.router.navigate(['/conversation', conversationId]);
  }
}
