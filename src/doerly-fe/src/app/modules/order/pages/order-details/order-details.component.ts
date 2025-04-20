import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { OrderStatus } from '../../domain/enums/order-status';
import { PaymentKind } from '../../domain/enums/payment-kind';
import { CommonModule } from '@angular/common';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastHelper } from 'app/@core/helpers/toast.helper';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss'],
  imports: [
    CommonModule,
    Card,
    Button,
    RouterLink,
    TranslatePipe
  ]
})
export class OrderDetailsComponent implements OnInit {
  order: GetOrderResponse | null = null;
  loading: boolean = true;
  profileId: number = 1;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private router: Router,
    private toastHelper: ToastHelper
  ) {}

  ngOnInit(): void {
    const orderId = Number(this.route.snapshot.paramMap.get('id'));
    if (orderId) {
      this.loading = true;
      this.orderService.getOrder(orderId).subscribe({
        next: (response) => {
          this.order = response.value || null;
          this.loading = false;
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        }
      });
    }
  }

  cancelOrder(): void {
    if (!this.order) return;

    this.orderService.cancelOrder(this.order.id).subscribe({
        next: () => {
          this.toastHelper.showSuccess('common.success', 'ordering.cancelled-successfully');
          this.router.navigate(['/ordering'], { queryParams: { tab: 2, subTab: 0 } });
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.toastHelper.showError('common.error', error.error.errorMessage);
          }
          else {
            this.toastHelper.showError('common.error', 'common.error-occurred');
          }
        }
    });
  }

  getOrderStatusString(status: OrderStatus): string {
    return OrderStatus[status];
  }

  getPaymentKindString(paymentKind: PaymentKind): string {
    return PaymentKind[paymentKind];
  }
}