import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { OrderService } from '../../domain/order.service';
import { CreateOrderRequest } from '../../models/requests/create-order-request';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { NgIf } from '@angular/common';
import { Card } from 'primeng/card';
import { getError, isInvalid, setServerErrors, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { SelectItem } from 'primeng/api';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { EPaymentKind } from '../../domain/enums/payment-kind';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { CreateOrderResponse } from '../../models/responses/create-order-response';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { ToastHelper } from 'app/@core/helpers/toast.helper';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss'],
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
export class CreateOrderComponent implements OnInit {

  createOrderForm!: FormGroup;
  paymentKinds: SelectItem[] = [];

  constructor(private formBuilder: FormBuilder,
              private orderService: OrderService,
              private router: Router,
              private translate: TranslateService,
              private readonly jwtTokenHelper: JwtTokenHelper) {}

  ngOnInit() {
    this.initForm();
    this.initPaymentKinds();
  }

  initForm(): void {
    this.createOrderForm = this.formBuilder.group({
      categoryId: ['', Validators.required],
      name: ['', [Validators.required, this.minimumLengthValidator(1), Validators.maxLength(100)]],
      description: ['', [Validators.required, this.minimumLengthValidator(5), Validators.maxLength(4000)]],
      price: ['', [Validators.required, Validators.min(0)]],
      paymentKind: [1, Validators.required],
      dueDate: ['', [Validators.required, this.dateValidator]]
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

  dateValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();
    if (selectedDate < currentDate) {
      return { 'invalidDate': true };
    }
    return null;
  }

  initPaymentKinds(): void {
    this.paymentKinds = Object.keys(EPaymentKind)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
        label: this.translate.instant('ordering.payment_kinds.' + key),
        value: EPaymentKind[key as keyof typeof EPaymentKind]
      }));
  }

  createOrder(): void {
    const request = this.createOrderForm.value as CreateOrderRequest;
    request.customerId = this.jwtTokenHelper.getUserInfo()?.id ?? 0;

    this.orderService.createOrder(request).subscribe({
      next: (response: BaseApiResponse<CreateOrderResponse>) => {
        const value = response.value;
        this.router.navigate([`ordering/order/${value!.id}`]);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400) {
          const errors = error.error.errors;
          setServerErrors(this.createOrderForm, errors);
        }
      }
    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}