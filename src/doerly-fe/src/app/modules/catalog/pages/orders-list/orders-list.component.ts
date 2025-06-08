import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { Avatar } from 'primeng/avatar';
import { Button } from 'primeng/button';
import { PaginatorModule } from 'primeng/paginator';
import { Tag } from 'primeng/tag';
import { DataView } from 'primeng/dataview';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandlerService } from 'app/@core/services/error-handler.service';
import { EOrderStatus, getOrderStatusSeverity } from 'app/modules/order/domain/enums/order-status';
import { OrderService } from 'app/modules/order/domain/order.service';
import { GetOrdersWithPaginationByPredicatesRequest } from 'app/modules/order/models/requests/get-orders-request';
import { GetOrderResponse } from 'app/modules/order/models/responses/get-order-response';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { GetOrdersWithFiltrationResponse } from '../../models/get-orders-with-filtration-response.model';
import { OrdersService } from '../../services/orders.service';
import { GetOrdersWithFiltrationRequest } from '../../models/get-orders-with-filtration-request.model';
import { BasePaginationResponse } from 'app/@core/models/base-pagination-response';

@Component({
  selector: 'app-orders-list',
  imports: [
    DataView,
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

  orders: GetOrdersWithFiltrationResponse[] = [];
  totalRecords: number = 0;
  categoryId?: number;
  loading: boolean = true;
  returnUrl!: string;
  EOrderStatus = EOrderStatus;
  public getOrderStatusSeverity = getOrderStatusSeverity;

  pagination = {
    pageNumber: 0,
    pageSize: 12,
    totalCount: 0
  };

  constructor(private ordersService: OrdersService,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandlerService
  ) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['return'];

    this.route.params.subscribe(params => {
      this.categoryId = params['categoryId'] ? +params['categoryId'] : undefined;
      this.pagination.pageNumber = 0;
      this.loadOrders();
    });
  }

  loadOrders() {
    this.loading = true;
    const request: GetOrdersWithFiltrationRequest = {
      pageInfo: {
        number: this.pagination.pageNumber + 1,
        size: this.pagination.pageSize
      },
      categoryId: this.categoryId!,
      isOrderByPrice: false,
      isDescending: false,
    };
    this.ordersService.getOrdersWithPagination(request).subscribe({
      next: (response: BasePaginationResponse<GetOrdersWithFiltrationResponse>) => {
        this.orders = response?.items || [];
        this.totalRecords = response?.count || 0;
        this.loading = false;
      },
      error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
    });
  }
}
