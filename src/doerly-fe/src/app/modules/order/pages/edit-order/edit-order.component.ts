import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslatePipe } from '@ngx-translate/core';
import { ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { NgIf } from '@angular/common';
import { Card } from 'primeng/card';
import { SelectItem } from 'primeng/api';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { EPaymentKind } from '../../domain/enums/payment-kind';
import { getError, isInvalid, setServerErrors, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { UpdateOrderRequest } from '../../models/requests/update-order-request';

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

  editOrderForm!: FormGroup;
  paymentKinds: SelectItem[] = [];
  orderId!: number;
  loading: boolean = true;

  constructor(
    private formBuilder: FormBuilder,
    private orderService: OrderService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.orderId = Number(this.route.snapshot.paramMap.get('id'));
    this.initForm();
    this.initPaymentKinds();
    this.loadOrderDetails();
  }

  initForm(): void {
    this.editOrderForm = this.formBuilder.group({
      categoryId: ['', Validators.required],
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(4000)]],
      price: ['', [Validators.required, Validators.min(0)]],
      paymentKind: [1, Validators.required],
      dueDate: ['', [Validators.required]]
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
    this.paymentKinds = Object.keys(EPaymentKind)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
        label: EPaymentKind[key as keyof typeof EPaymentKind].toString(),
        value: EPaymentKind[key as keyof typeof EPaymentKind]
      }));
  }

  loadOrderDetails(): void {
    this.orderService.getOrder(this.orderId).subscribe({
      next: (response) => {
        const order = response.value;
        if (order) {
          this.editOrderForm.patchValue({
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
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  updateOrder(): void {
    if (this.editOrderForm.invalid) return;

    const updatedOrder: UpdateOrderRequest = this.editOrderForm.value;
    this.orderService.updateOrder(this.orderId, updatedOrder).subscribe({
      next: () => {
        this.router.navigate(['/ordering/order', this.orderId]);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400) {
          const errors = error.error.errors;
          setServerErrors(this.editOrderForm, errors);
        }
      }
    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}