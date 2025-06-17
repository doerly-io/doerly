import {inject, Pipe, PipeTransform} from '@angular/core';
import {EPaymentStatus} from 'app/modules/payments/enums/e-payment-status';
import {TranslateService, TranslationObject} from '@ngx-translate/core';
import {Observable} from 'rxjs';
import {Translation} from 'primeng/api';

@Pipe({
  name: 'paymentStatus'
})
export class PaymentStatusPipe implements PipeTransform {

  private readonly translateService: TranslateService = inject(TranslateService);

  transform(value: number, ...args: unknown[]): Observable<Translation | TranslationObject> {
    switch (value) {
      case EPaymentStatus.Pending:
        return this.translateService.get('payments.status.pending');
      case EPaymentStatus.Failed:
        return this.translateService.get('payments.status.failed');
      case EPaymentStatus.Error:
        return this.translateService.get('payments.status.error');
      case EPaymentStatus.Expired:
        return this.translateService.get('payments.status.expired');
      case EPaymentStatus.Completed:
        return this.translateService.get('payments.status.completed');
      default:
        return this.translateService.get('payments.status.unknown', {status: value});
    }
  }

}
