import {Component, inject, input, OnInit, signal} from '@angular/core';
import {Scroller} from 'primeng/scroller';
import {AsyncPipe, DatePipe, NgIf} from '@angular/common';
import {Button} from 'primeng/button';
import {Panel} from 'primeng/panel';
import {PaymentCurrencyPipe} from 'app/modules/payments/pipe/payment-currency.pipe';
import {PaymentStatusPipe} from 'app/modules/payments/pipe/payment-status.pipe';
import {PrimeTemplate} from 'primeng/api';
import {Tag} from 'primeng/tag';
import {TranslatePipe} from '@ngx-translate/core';
import {ProfileService} from 'app/modules/profile/domain/profile.service';
import {ToastHelper} from 'app/@core/helpers/toast.helper';
import {CursorPaginationRequest} from 'app/@core/models/cursor-pagination-request';
import {CursorPaginationResponse} from 'app/@core/models/cursor-pagination-response';
import {OrderFeedbackResponse} from 'app/modules/order/models/responses/feedback/order-feedback-response';
import {FeedbackInputComponent} from 'app/modules/order/components/feedback-input/feedback-input.component';

@Component({
  selector: 'app-feedback-history',
  imports: [
    Scroller,
    AsyncPipe,
    Button,
    DatePipe,
    NgIf,
    Panel,
    PaymentCurrencyPipe,
    PaymentStatusPipe,
    PrimeTemplate,
    TranslatePipe,
    FeedbackInputComponent
  ],
  templateUrl: './feedback-history.component.html',
  styleUrl: './feedback-history.component.scss'
})
export class FeedbackHistoryComponent implements OnInit {

  private readonly profileService: ProfileService = inject(ProfileService);
  private readonly toastHelper: ToastHelper = inject(ToastHelper);

  protected readonly loading = signal(false);

  public feedbacks = signal<OrderFeedbackResponse[]>([]);

  public userId = input<number>(0);

  protected hasMoreFeedbacks = signal<boolean>(false);

  private paginationRequest: CursorPaginationRequest = {
    pageSize: 5,
    cursor: ''
  }

  ngOnInit(): void {
    this.loadFeedbacks();
  }

  loadMoreClicked() {
    this.loadFeedbacks();
  }

  loadFeedbacks(): void {
    this.loading.set(true);
    this.profileService.getFeedbacksHistory(this.userId(), this.paginationRequest).subscribe({
      next: (response: CursorPaginationResponse<OrderFeedbackResponse>) => {
        this.feedbacks.update((value) => [...value, ...response?.items!]);
        this.paginationRequest.cursor = response?.cursor;
        this.hasMoreFeedbacks.set(!!this.paginationRequest.cursor);
      },
      error: (error) => {
        this.toastHelper.showApiError(error, 'profile.paymentHistory.errors.load');
      },
      complete: () => {
        this.loading.set(false);
      }
    });
  }

}
