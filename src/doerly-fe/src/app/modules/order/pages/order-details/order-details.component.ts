import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { OrderStatus } from '../../domain/enums/order-status';
import { PaymentKind } from '../../domain/enums/payment-kind';
import { CommonModule } from '@angular/common';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';

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
    private orderService: OrderService
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

  getOrderStatusString(status: OrderStatus): string {
    return OrderStatus[status];
  }

  getPaymentKindString(paymentKind: PaymentKind): string {
    return PaymentKind[paymentKind];
  }
}