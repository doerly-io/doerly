import {EPaymentStatus} from 'app/modules/payments/enums/e-payment-status';
import {ECurrency} from 'app/modules/payments/enums/e-currency';

export interface PaymentHistoryItemResponse {
  paymentId: number;
  createdAt: string;
  status: EPaymentStatus;
  amount: number;
  currency: ECurrency;
  description: string;
  billId: string;
  billDescription: string;
}
