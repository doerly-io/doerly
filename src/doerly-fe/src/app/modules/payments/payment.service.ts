import {Injectable} from '@angular/core';
import {BaseCheckoutResponse} from './models/base-checkout-response';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {BaseApiResponse} from '../../@core/models/base-api-response';
import {Observable} from 'rxjs';
import {CursorPaginationRequest} from 'app/@core/models/cursor-pagination-request';
import {CursorPaginationResponse} from 'app/@core/models/cursor-pagination-response';
import {PaymentHistoryItemResponse} from 'app/modules/payments/models/payment-history-item-response';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private baseUrl = environment.baseApiUrl + '/payments';

  constructor(private httpClient: HttpClient) {
  }

  testPayment() {
    this.httpClient.get<BaseApiResponse<BaseCheckoutResponse>>(`${this.baseUrl}/payments/test`).subscribe(
      (response: any) => {
        this.processCheckoutResponse(response.value);
      },
      (error) => {
        console.error('Payment test error:', error);
      }
    );

  }

  processCheckoutResponse(checkout: BaseCheckoutResponse) {
    window.open(checkout.checkoutUrl, '_blank');
  }

  getUserPayments(paginationRequest: CursorPaginationRequest): Observable<CursorPaginationResponse<PaymentHistoryItemResponse>> {
    return this.httpClient.get<CursorPaginationResponse<PaymentHistoryItemResponse>>(`${this.baseUrl}/payments/user`, {
      params: {
        pageSize: paginationRequest.pageSize,
        cursor: paginationRequest.cursor || ''
      }
    });
  }

}
