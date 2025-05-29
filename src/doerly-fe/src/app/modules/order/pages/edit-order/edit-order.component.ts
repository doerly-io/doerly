import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { NgIf } from '@angular/common';
import { Card } from 'primeng/card';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { EPaymentKind } from '../../domain/enums/payment-kind';
import { getError, isInvalid, setServerErrors, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { CreateOrderRequest } from '../../models/requests/create-order-request';
import { ToastHelper } from 'app/@core/helpers/toast.helper';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TranslatePipe,
    ButtonDirective,
    InputText,
    DatePickerModule,
    SelectModule,
    NgIf,
    Card
  ]
})
export class EditOrderComponent implements OnInit {
  orderForm!: FormGroup;
  paymentKinds: any[] = [];
  orderId?: number;
  isEdit: boolean = false;
  loading: boolean = false;
  currentDate: Date = new Date();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private translate: TranslateService,
    private toastHelper: ToastHelper
  ) { }

  ngOnInit() {
    this.orderId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.orderId;

    this.initForm();
    this.initPaymentKinds();

    if (this.isEdit) {
      this.loading = true;
      this.orderService.getOrder(this.orderId!).subscribe({
        next: (response: BaseApiResponse<GetOrderResponse>) => {
          const order = response.value;
          if (order) {
            this.orderForm.patchValue({
              categoryId: order.categoryId,
              name: order.name,
              description: order.description,
              price: order.price,
              paymentKind: order.paymentKind,
              dueDate: new Date(order.dueDate)
            });
          }
          this.loading = false;
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
  }

  initForm() {
    this.orderForm = this.formBuilder.group({
      categoryId: ['', Validators.required],
      name: ['', [Validators.required, this.minimumLengthValidator(1), Validators.maxLength(100)]],
      description: ['', [Validators.required, this.minimumLengthValidator(5), Validators.maxLength(4000)]],
      price: [1, [Validators.required, Validators.min(1)]],
      paymentKind: [1, Validators.required],
      dueDate: ['', Validators.required]
    });
  }

  minimumLengthValidator(minLength: number) {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value && control.value.length < minLength) {
        return { 'minimumLength': true };
      }
      return null;
    };
  }

  initPaymentKinds() {
    this.paymentKinds = Object.keys(EPaymentKind)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
        label: this.translate.instant('ordering.payment_kinds.' + key),
        value: EPaymentKind[key as keyof typeof EPaymentKind]
      }));
  }

  submit() {
    if (this.orderForm.invalid) 
      return;
    
    if (this.isEdit) {
      this.orderService.updateOrder(this.orderId!, this.orderForm.value)
      .subscribe({
        next: () => {
          this.router.navigate(['/ordering/order', this.orderId]);
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
    } else {
      const request = this.orderForm.value as CreateOrderRequest;
      this.orderService.createOrder(request)
      .subscribe({
        next: (response: BaseApiResponse<number>) => {
        this.router.navigate(['/ordering/order', response.value]);
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
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}