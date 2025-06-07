import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { EOrderStatus, getOrderStatusSeverity } from '../../domain/enums/order-status';
import { EPaymentKind } from '../../domain/enums/payment-kind';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { UpdateOrderStatusRequest } from '../../models/requests/update-order-status-request';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { UpdateOrderStatusResponse } from '../../models/responses/update-order-status-response';
import { PanelModule } from 'primeng/panel';
import { Tag } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { DividerModule } from 'primeng/divider';
import { TooltipModule } from 'primeng/tooltip';
import { ImageModule } from 'primeng/image';
import { GalleriaModule } from 'primeng/galleria';
import { ErrorHandlerService } from '../../domain/error-handler.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss'],
  imports: [
    CommonModule,
    Card,
    Button,
    RouterLink,
    TranslatePipe,
    PanelModule,
    Tag,
    AvatarModule,
    DividerModule,
    TooltipModule,
    NgOptimizedImage,
    ImageModule,
    GalleriaModule
  ]
})
export class OrderDetailsComponent implements OnInit {
  order: GetOrderResponse | null = null;
  loading: boolean = true;
  profileId: number;
  EOrderStatus = EOrderStatus;
  EPaymentKind = EPaymentKind;
  public getOrderStatusSeverity = getOrderStatusSeverity;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private router: Router,
    private toastHelper: ToastHelper,
    private readonly jwtTokenHelper: JwtTokenHelper,
    private errorHandler: ErrorHandlerService
  ) {
    this.profileId = this.jwtTokenHelper.getUserInfo()?.id ?? 0;
  }

  ngOnInit(): void {
    const orderId = Number(this.route.snapshot.paramMap.get('id'));
    if (orderId) {
      this.loading = true;
      this.orderService.getOrder(orderId).subscribe({
        next: (response) => {
          this.order = response.value || null;
          this.loading = false;
        },
        error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
      });
    }
  }

  updateOrderStatus(status: EOrderStatus): void {
    if (!this.order) return;

    const updateOrderStatusRequest: UpdateOrderStatusRequest = {
      status: status,
      returnUrl: `${window.location.origin}/ordering/orders/${this.order.id}`,
    };

    this.orderService.updateOrderStatus(this.order.id, updateOrderStatusRequest).subscribe({
      next: (response: BaseApiResponse<UpdateOrderStatusResponse>) => {
        const value = response.value;
        if (value?.paymentUrl) {
          this.toastHelper.showInfo('common.info', 'ordering.payment_redirect');
          setTimeout(() => {
            window.location.href = value.paymentUrl!;
          }, 3000);
        }
        else {
          this.toastHelper.showSuccess('common.success', 'ordering.cancelled_successfully');
          this.router.navigate(['/ordering'], { queryParams: { tab: 2, subTab: 0 } });
        }
      },
      error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
    });
  }

  get canEdit(): boolean {
    return this.order?.status === EOrderStatus.Placed && this.order?.customerId === this.profileId;
  }
  get canCancel(): boolean {
    return this.order?.status === EOrderStatus.Placed && this.order?.customerId === this.profileId;
  }
  get canSendProposal(): boolean {
    return this.order?.customerId !== this.profileId && this.order?.status === EOrderStatus.Placed;
  }
  get canEnd(): boolean {
    return (
      (this.order?.status === EOrderStatus.InProgress && (this.order?.customerId === this.profileId || this.order?.executorId === this.profileId)) ||
      (this.order?.status === EOrderStatus.AwaitingPayment && this.order?.executorId === this.profileId) ||
      (this.order?.status === EOrderStatus.AwaitingConfirmation && !this.order?.executorCompletionConfirmation && this.order?.executorId === this.profileId)
    );
  }
  get canPay(): boolean {
    return this.order?.status === EOrderStatus.AwaitingPayment && this.order?.customerId === this.profileId;
  }

}