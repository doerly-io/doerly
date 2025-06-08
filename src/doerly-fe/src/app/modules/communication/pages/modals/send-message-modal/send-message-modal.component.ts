import {Component, EventEmitter, Input, Output, inject, numberAttribute} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import {CommunicationService} from '../../../domain/communication.service';
import {Textarea} from 'primeng/textarea';

@Component({
  selector: 'app-send-message-modal',
  standalone: true,
  imports: [
    DialogModule,
    ButtonModule,
    ReactiveFormsModule,
    TranslateModule,
    ToastModule,
    Textarea
  ],
  providers: [MessageService],
  template: `
    <p-dialog
      [(visible)]="visible"
      [modal]="true"
      [header]="'communication.dialog.title' | translate"
      [style]="{ width: '32rem' }"
      (close)="onClose()"
      (onHide)="onClose()"
    >
      <form [formGroup]="messageForm" class="d-flex flex-column gap-3">
        <div class="d-flex flex-column gap-2">
          <label for="messageContent" class="fw-semibold">
            {{ 'communication.dialog.content.label' | translate }}
          </label>
          <textarea
            id="messageContent"
            pTextarea
            formControlName="messageContent"
            [rows]="5"
            [placeholder]="'communication.dialog.content.placeholder' | translate"
            style="width: 100%"
          ></textarea>
        </div>
      </form>

      <ng-template pTemplate="footer">
        <div class="d-flex justify-content-end gap-2">
          <button
            pButton
            type="button"
            class="p-button-text"
            (click)="onClose()"
            [label]="'common.cancel' | translate"
          ></button>
          <button
            pButton
            type="button"
            class="p-button-primary"
            (click)="onSendMessage()"
            [label]="'communication.dialog.send' | translate"
            [disabled]="!messageForm.valid"
          >
            <i class="pi pi-send me-2"></i>
          </button>
        </div>
      </ng-template>
    </p-dialog>
    <p-toast></p-toast>
  `
})
export class SendMessageModalComponent {
  @Input() visible = false;
  @Input({transform: numberAttribute}) recipientId!: number;
  @Output() visibleChange = new EventEmitter<boolean>();

  messageForm: FormGroup;
  isLoading = false;

  private readonly formBuilder = inject(FormBuilder);
  private readonly communicationService = inject(CommunicationService);
  private readonly messageService = inject(MessageService);
  private readonly translate = inject(TranslateService);
  private readonly router = inject(Router);

  constructor() {
    this.messageForm = this.formBuilder.group({
      messageContent: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  onClose(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.messageForm.reset();
  }

  onSendMessage(): void {
    if (this.messageForm.invalid || !this.recipientId) return;

    this.isLoading = true;
    const messageContent = this.messageForm.get('messageContent')?.value;

    let conversationId: number | null;

    firstValueFrom(this.communicationService.getConversationWithUser(this.recipientId))
      .then(existingConversationResult => {
        if (!existingConversationResult.isSuccess) {
          throw new Error(existingConversationResult.errorMessage ?? this.translate.instant('communication.message.error.detail'));
        }

        conversationId = existingConversationResult.value;
        if (conversationId) {
          return conversationId;
        }

        return firstValueFrom(this.communicationService.createConversation(this.recipientId))
          .then(newConversationResult => {
            if (!newConversationResult.isSuccess) {
              throw new Error(newConversationResult.errorMessage ?? this.translate.instant('communication.message.error.detail'));
            }
            conversationId = newConversationResult.value;
            return conversationId;
          });
      })
      .then(() => {
        if (!conversationId) {
          throw new Error(this.translate.instant('communication.message.error.no_conversation'));
        }
        return firstValueFrom(this.communicationService.sendMessage({
          conversationId: conversationId!,
          messageContent: messageContent
        }));
      })
      .then(messageResult => {
        if (!messageResult.isSuccess) {
          throw new Error(messageResult.errorMessage ?? this.translate.instant('communication.message.error.detail'));
        }

        this.messageService.add({
          severity: 'success',
          summary: this.translate.instant('communication.message.success.title'),
          detail: this.translate.instant('communication.message.success.detail')
        });

        this.visible = false;
        this.messageForm.reset();
        this.router.navigate(['/communication/conversations'], {
          queryParams: { conversationId: conversationId }
        });
      })
      .catch(error => {
        this.messageService.add({
          severity: 'error',
          summary: this.translate.instant('communication.message.error.title'),
          detail: error.message || this.translate.instant('communication.message.error.detail')
        });
      })
      .finally(() => {
        this.isLoading = false;
      });
  }
}
