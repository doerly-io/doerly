import { Component, Input, OnInit } from '@angular/core';
import { OrderService } from '../../domain/order.service';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { DataView } from 'primeng/dataview';
import { Tag } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { GetOrdersWithPaginationByPredicatesRequest } from '../../models/requests/get-orders-request';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { PaginatorModule } from 'primeng/paginator';
import { OrderStatus } from '../../domain/enums/order-status';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-orders-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule,
    Button,
    TranslatePipe,
    RouterLink
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.scss'
})
export class OrdersListComponent implements OnInit {

  @Input() customerId?: number | null;
  @Input() executorId?: number | null;
  @Input() canCreateOrder: boolean = false;

  orders: GetOrderResponse[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  returnUrl!: string;

  constructor(private orderService: OrderService,
                private router: Router,
                private route: ActivatedRoute) {}

  ngOnInit() { 
    this.returnUrl = this.route.snapshot.queryParams['return'];
  }

  loadOrders(event: any) {
    this.loading = true;
    const request: GetOrdersWithPaginationByPredicatesRequest = {
      pageInfo: {
        number: event.first / event.rows + 1,
        size: event.rows
      },
      customerId: this.customerId,
      executorId: this.executorId
    };
    this.orderService.getOrdersWithPagination(request).subscribe({
      next: (response) => {
        this.orders = response.value?.orders || [];
        this.totalRecords = response.value?.total || 0;
        this.loading = false;
      },
      error: (error) => {
        console.log(error);
        this.loading = false;
      }
    });
  }

  getOrderStatusString(status: OrderStatus): string {
      return OrderStatus[status];
  }

  getOrderStatusSeverity(status: OrderStatus): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
      switch (status) {
        case OrderStatus.Placed:
          return 'info';
        case OrderStatus.InProgress:
          return 'warn';
        case OrderStatus.Completed:
          return 'success';
        case OrderStatus.Canceled:
          return 'danger';
        default:
          return 'secondary';
      }
    }

  navigateToOrderDetails(orderId: number): void {
    this.router.navigate(['/ordering/order', orderId]);
  }
}