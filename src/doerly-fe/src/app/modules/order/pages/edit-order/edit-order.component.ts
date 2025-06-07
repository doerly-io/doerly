import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../domain/order.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { Button, ButtonDirective } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { CommonModule, NgIf } from '@angular/common';
import { Card } from 'primeng/card';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { EPaymentKind } from '../../domain/enums/payment-kind';
import { getError, isInvalid, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { BaseApiResponse } from 'app/@core/models/base-api-response';
import { GetOrderResponse } from '../../models/responses/get-order-response';
import { CreateOrderRequest } from '../../models/requests/create-order-request';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { CreateOrderResponse } from '../../models/responses/create-order-response';
import { Checkbox } from 'primeng/checkbox';
import { FileUploadModule } from 'primeng/fileupload';
import { Badge } from 'primeng/badge';
import { FileInfoModel } from '../../models/responses/file-info-model';
import { Tooltip } from 'primeng/tooltip';
import { Textarea } from 'primeng/textarea';
import { ImageModule } from 'primeng/image';
import { AddressSelectComponent } from "../../../../@shared/components/address-select/address-select.component";
import { ErrorHandlerService } from '../../domain/error-handler.service';

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
    Card,
    Checkbox,
    FileUploadModule,
    Button,
    Badge,
    CommonModule,
    Tooltip,
    Textarea,
    ImageModule,
    AddressSelectComponent
]
})
export class EditOrderComponent implements OnInit {
  orderForm!: FormGroup;
  paymentKinds: any[] = [];
  orderId?: number;
  isEdit: boolean = false;
  loading: boolean = false;
  currentDate: Date = new Date();

  totalSizeLimit: number = 10 * 1024 * 1024; // 10 MB
  files: File[] = [];
  existingFiles: FileInfoModel[] = [];
  totalFilesSize: number = 0;
  allowedSizeForUpload: number = 0;
  addressReady: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private translate: TranslateService,
    private toastHelper: ToastHelper,
    private errorHandler: ErrorHandlerService
  ) { }

  ngOnInit() {
    this.orderId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.orderId;

    this.initForm();
    this.initPaymentKinds();

    this.orderForm.setValidators(() => this.filesTotalSizeValidator());

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
              dueDate: new Date(order.dueDate),
              isPriceNegotiable: order.isPriceNegotiable,
              useProfileAddress: order.useProfileAddress,
              regionId: order.addressInfo?.regionId ?? null,
              cityId: order.addressInfo?.cityId ?? null
            });
            this.addressReady = true;
            this.existingFiles = order.existingFiles || [];
            this.updateFilesSizeAndLimit();
          }
          this.loading = false;
        },
        error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
      });
    } else {
      this.updateFilesSizeAndLimit();
      this.addressReady = true;
    }
  }

  initForm() {
    this.orderForm = this.formBuilder.group({
      categoryId: ['', Validators.required],
      name: ['', [Validators.required, this.minimumLengthValidator(1), Validators.maxLength(100)]],
      description: ['', [Validators.required, this.minimumLengthValidator(5), Validators.maxLength(4000)]],
      price: [1, [Validators.required, Validators.min(1)]],
      paymentKind: [1, Validators.required],
      dueDate: ['', Validators.required],
      isPriceNegotiable: [false, Validators.required],
      useProfileAddress: [false, Validators.required],
      regionId: ['', Validators.required],
      cityId: ['', Validators.required]
    });

    // Динамічна валідація
    this.orderForm.get('useProfileAddress')!.valueChanges.subscribe((useProfile: boolean) => {
      if (!useProfile) {
        this.orderForm.get('regionId')!.setValidators([Validators.required]);
        this.orderForm.get('cityId')!.setValidators([Validators.required]);
        // Скидаємо значення при перемиканні
        //this.orderForm.patchValue({ regionId: null, cityId: null });
      } else {
        this.orderForm.get('regionId')!.clearValidators();
        this.orderForm.get('cityId')!.clearValidators();
        //this.orderForm.patchValue({ regionId: null, cityId: null });
      }
      this.orderForm.get('regionId')!.updateValueAndValidity();
      this.orderForm.get('cityId')!.updateValueAndValidity();
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

  filesTotalSizeValidator() {
    const total = this.getTotalFilesSize();
    return total > this.totalSizeLimit ? { filesSizeLimitExceeded: true } : null;
  }

  onSelectedFiles(event: any) {
    let added = false;
    for (const file of event.currentFiles) {
      if (!this.files.includes(file)) {
        this.files.push(file);
        added = true;
      }
    }
    this.updateFilesSizeAndLimit();
    if (this.getTotalFilesSize() > this.totalSizeLimit) {
      this.toastHelper.showWarn(
        'ordering.files_size_limit_exceeded',
        this.translate.instant('ordering.files_size_limit', { size: this.formatSize(this.totalSizeLimit) })
      );
    }
  }

  onClearTemplatingUpload(clear: Function) {
    clear();
    this.files = [];
    this.existingFiles = [];
    this.updateFilesSizeAndLimit();
  }

  removeExistingFile(file: FileInfoModel) {
    this.existingFiles = this.existingFiles.filter(f => f.filePath !== file.filePath);
    this.updateFilesSizeAndLimit();
  }

  onRemoveTemplatingFile(event: Event, file: File, removeFileCallback: Function) {
    removeFileCallback(file);
    this.files = this.files.filter(f => f !== file);
    this.updateFilesSizeAndLimit();
  }

  updateFilesSizeAndLimit() {
    this.totalFilesSize = this.getTotalFilesSize();
    this.allowedSizeForUpload = this.totalSizeLimit - this.getExistingFilesSize();
    this.orderForm.updateValueAndValidity();
  }

  getTotalFilesSize(): number {
    return this.getExistingFilesSize() + this.getNewFilesSize();
  }

  getExistingFilesSize(): number {
    return this.existingFiles.reduce((sum, f) => sum + (f.fileSize || 0), 0);
  }

  getNewFilesSize(): number {
    return this.files.reduce((sum, f) => sum + f.size, 0);
  }

  formatSize(bytes: number): string {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const dm = 2;
    const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  submit() {
    if (this.orderForm.invalid)
      return;

    if (this.isEdit) {
      this.orderService.updateOrder(this.orderId!, this.orderForm.value, this.files, this.existingFiles)
        .subscribe({
          next: () => {
            this.router.navigate(['/ordering/order', this.orderId]);
          },
          error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
        });
    } else {
      this.orderService.createOrder(this.orderForm.value, this.files)
        .subscribe({
          next: (response: BaseApiResponse<CreateOrderResponse>) => {
            this.router.navigate(['/ordering/order', response.value!.id]);
          },
          error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
        });
    }
  }

  onAddressChange(address: { cityId: number, regionId?: number }) {
    this.orderForm.patchValue({
      cityId: address.cityId,
      regionId: address.regionId ?? this.orderForm.value.regionId
    });
  }

  initPaymentKinds() {
    this.paymentKinds = Object.keys(EPaymentKind)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
        label: this.translate.instant('ordering.payment_kinds.' + key),
        value: EPaymentKind[key as keyof typeof EPaymentKind]
      }));
  }

  choose(event: Event, callback: Function) {
    callback();
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}