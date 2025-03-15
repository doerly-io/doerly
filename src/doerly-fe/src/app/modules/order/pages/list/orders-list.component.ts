import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../domain/order.service';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { DataView } from 'primeng/dataview';
import { Tag } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { GetItemsWithPaginationRequest } from '../../models/requests/get-items-with-pagination-request';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { PaginatorModule } from 'primeng/paginator';
import { OrderStatus } from '../../domain/enums/order-status';

@Component({
  selector: 'app-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.scss'
})
export class OrdersListComponent implements OnInit {

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
    const request: GetItemsWithPaginationRequest = {
      pageInfo: {
        number: event.first / event.rows + 1,
        size: event.rows
      }
    };
    this.orderService.getOrdersWithPagination(request).subscribe({
      next: (response) => {
        this.orders = response.value?.orders || [];
        this.totalRecords = response.value?.total || 0;
        this.loading = false;
        console.log(this.orders);
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

  navigateToOrderDetails(orderId: number): void {
    this.router.navigate(['/ordering/order', orderId]);
  }
}