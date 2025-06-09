import {Component, inject, OnInit, signal} from '@angular/core';
import {CursorPaginationRequest} from 'app/@core/models/cursor-pagination-request';
import {ToastHelper} from 'app/@core/helpers/toast.helper';
import {CursorPaginationResponse} from 'app/@core/models/cursor-pagination-response';
import {ScrollPanel} from 'primeng/scrollpanel';
import {AsyncPipe, CurrencyPipe, DatePipe, NgIf} from '@angular/common';
import {Button} from 'primeng/button';
import {PaymentHistoryItemResponse} from 'app/modules/payments/models/payment-history-item-response';
import {PaymentStatusPipe} from 'app/modules/payments/pipe/payment-status.pipe';
import {PaymentCurrencyPipe} from 'app/modules/payments/pipe/payment-currency.pipe';
import {Panel} from 'primeng/panel';
import {Tag} from 'primeng/tag';
import {EPaymentStatus} from 'app/modules/payments/enums/e-payment-status';
import {Scroller} from 'primeng/scroller';
import {PrimeTemplate} from 'primeng/api';
import {TranslatePipe} from '@ngx-translate/core';
import {ProfileService} from 'app/modules/profile/domain/profile.service';

@Component({
  selector: 'app-payment-history',
  imports: [
    NgIf,
    Button,
    DatePipe,
    PaymentStatusPipe,
    PaymentCurrencyPipe,
    AsyncPipe,
    Panel,
    Tag,
    Scroller,
    PrimeTemplate,
    TranslatePipe,
  ],
  templateUrl: './payment-history.component.html',
  styleUrl: './payment-history.component.scss',
  providers: [CurrencyPipe]
})
export class PaymentHistoryComponent implements OnInit {

  private readonly profileService: ProfileService = inject(ProfileService);
  private readonly toastHelper: ToastHelper = inject(ToastHelper);

  protected readonly loading = signal(false);

  protected payments = signal<PaymentHistoryItemResponse[]>([]);
  protected hasMorePayments = signal<boolean>(false);

  private paginationRequest: CursorPaginationRequest = {
    pageSize: 5,
    cursor: ''
  }

  ngOnInit(): void {
    this.loadPaymentHistory();
  }

  loadMoreClicked() {
    this.loadPaymentHistory();
  }

  loadPaymentHistory(): void {
    this.loading.set(true);
    this.profileService.getPaymentsHistory(this.paginationRequest).subscribe({
      next: (response: CursorPaginationResponse<PaymentHistoryItemResponse>) => {
        this.payments.update((value) => [...value, ...response?.items!]);
        this.paginationRequest.cursor = response?.cursor;
        this.hasMorePayments.set(!!this.paginationRequest.cursor);
      },
      error: (error) => {
        this.toastHelper.showApiError(error, 'profile.paymentHistory.errors.load');
      },
      complete: () => {
        this.loading.set(false);
      }
    });
  }

  protected getSeverity(status: number): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (status) {
      case EPaymentStatus.Completed:
        return 'success';
      case EPaymentStatus.Pending:
        return 'info';
      case EPaymentStatus.Failed:
      case EPaymentStatus.Error:
        return 'danger';
      default:
        return 'warn';
    }
  }


}
