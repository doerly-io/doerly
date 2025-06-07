import { Component, Input, OnInit } from '@angular/core';
import { OrderService } from '../../domain/order.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DataView } from 'primeng/dataview';
import { Tag } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { GetOrdersWithPaginationByPredicatesRequest } from '../../models/requests/get-orders-request';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { PaginatorModule } from 'primeng/paginator';
import { EOrderStatus } from '../../domain/enums/order-status';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { getOrderStatusSeverity } from '../../domain/enums/order-status';
import { Avatar } from 'primeng/avatar';
import { ErrorHandlerService } from '../../domain/error-handler.service';

@Component({
  selector: 'app-orders-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule,
    Button,
    TranslatePipe,
    RouterLink,
    Avatar
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
  EOrderStatus = EOrderStatus;
  public getOrderStatusSeverity = getOrderStatusSeverity;

  constructor(private orderService: OrderService,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandlerService  
  ) { }

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
      error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
    });
  }
}