import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { OrderService } from '../../domain/order.service';
import { CreateOrderRequest } from '../../models/requests/create-order-request';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { NgIf } from '@angular/common';
import { Card } from 'primeng/card';
import { getError, isInvalid, setServerErrors, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { SelectItem } from 'primeng/api';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { PaymentKind, getPaymentKindString } from '../../domain/enums/payment-kind';

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
              private router: Router) {}

  ngOnInit() {
    this.initForm();
    this.initPaymentKinds();
  }

  initForm(): void {
    this.createOrderForm = this.formBuilder.group({
      categoryId: ['', Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
      paymentKind: [0, Validators.required],
      dueDate: ['', [Validators.required, this.dateValidator]]
    });
  }

  dateValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    if (selectedDate < currentDate) {
      return { 'invalidDate': true };
    }
    return null;
  }

  initPaymentKinds(): void {
    this.paymentKinds = Object.keys(PaymentKind)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
        label: getPaymentKindString(PaymentKind[key as keyof typeof PaymentKind]),
        value: PaymentKind[key as keyof typeof PaymentKind]
      }));
  }

  createOrder(): void {
    const request = this.createOrderForm.value as CreateOrderRequest;
    request.customerId = 2;

    this.orderService.createOrder(request).subscribe({
      next: (value) => {
        this.router.navigate(['/ordering'], { queryParams: { tab: 2, subTab: 0 } });
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