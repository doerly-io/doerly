import {
  Component,
  inject, input,
  Input,
  OnInit, signal
} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Card} from 'primeng/card';
import {Rating} from 'primeng/rating';
import {Textarea} from 'primeng/textarea';
import {ButtonDirective} from 'primeng/button';
import {NgIf} from '@angular/common';
import {TranslatePipe} from '@ngx-translate/core';
import {
  getError,
  getServersideError,
  isInvalid
} from 'app/@core/helpers/input-validation-helpers';
import {ToastHelper} from 'app/@core/helpers/toast.helper';
import {OrderService} from 'app/modules/order/domain/order.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ErrorHandlerService} from 'app/@core/services/error-handler.service';
import {OrderFeedbackResponse} from 'app/modules/order/models/responses/feedback/order-feedback-response';
import {CreateFeedbackRequest} from 'app/modules/order/models/requests/feedback/create-feedback-request';

@Component({
  selector: 'app-feedback-input',
  standalone: true,
  imports: [
    Card,
    Rating,
    ReactiveFormsModule,
    Textarea,
    ButtonDirective,
    NgIf,
    TranslatePipe
  ],
  templateUrl: './feedback-input.component.html',
  styleUrl: './feedback-input.component.scss'
})
export class FeedbackInputComponent implements OnInit {

  // Signal-based input
  public orderId = input<number>();
  public isReadonly = input<boolean>(false);

  protected isReadonlyMode = signal<boolean>(this.isReadonly());

  // Use internal field to retain state
  private _feedback?: OrderFeedbackResponse;

  @Input()
  set feedback(value: OrderFeedbackResponse | undefined) {
    console.log(value);
    this._feedback = value;
    if (this.feedbackForm) {
      this.patchForm(value);
      this.isReadonlyMode.set(value !== undefined);
    }
  }

  protected toastHelper = inject(ToastHelper);
  protected orderService = inject(OrderService);
  protected errorHandlerService = inject(ErrorHandlerService);

  feedbackForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    if (this._feedback) {
      this.patchForm(this._feedback);
    }
  }

  private initForm(): void {
    this.feedbackForm = this.fb.group({
      rating: [null, Validators.required],
      comment: [null, [Validators.required, Validators.maxLength(2000)]]
    });
  }

  private patchForm(feedback: OrderFeedbackResponse | undefined): void {
    if (!feedback) return;
    this.feedbackForm.patchValue({
      rating: feedback.rating,
      comment: feedback.comment
    });
  }

  get comment() {
    return this.feedbackForm.get('comment');
  }

  toggleEditMode() {
    this.isReadonlyMode.set(!this.isReadonlyMode());
  }

  onSubmit() {
    if (this.feedbackForm.valid) {
      const createdFeedbackRequest = this.feedbackForm.value as CreateFeedbackRequest;
      const orderId = this.orderId()!;
      this.orderService.createOrderFeedback(orderId, createdFeedbackRequest).subscribe({
        next: () => {
        },
        error: (error: HttpErrorResponse) => this.errorHandlerService.handleApiError(error)
      });
    } else {
      this.feedbackForm.markAllAsTouched();
    }
  }

  protected readonly getServersideError = getServersideError;
  protected readonly isInvalid = isInvalid;
  protected readonly getError = getError;
}
