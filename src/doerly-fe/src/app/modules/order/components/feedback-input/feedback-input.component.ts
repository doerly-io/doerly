import {Component, inject, input, Input, OnInit, signal} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Card} from 'primeng/card';
import {Rating} from 'primeng/rating';
import {Textarea} from 'primeng/textarea';
import {ButtonDirective} from 'primeng/button';
import {DatePipe, NgClass, NgIf, NgOptimizedImage} from '@angular/common';
import {TranslatePipe} from '@ngx-translate/core';
import {getError, getServersideError, isInvalid} from 'app/@core/helpers/input-validation-helpers';
import {ToastHelper} from 'app/@core/helpers/toast.helper';
import {OrderService} from 'app/modules/order/domain/order.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ErrorHandlerService} from 'app/@core/services/error-handler.service';
import {OrderFeedbackResponse} from 'app/modules/order/models/responses/feedback/order-feedback-response';
import {CreateFeedbackRequest} from 'app/modules/order/models/requests/feedback/create-feedback-request';
import {RouterLink} from '@angular/router';
import {Avatar} from 'primeng/avatar';

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
        TranslatePipe,
        NgClass,
        NgOptimizedImage,
        RouterLink,
        Avatar,
        DatePipe
    ],
    templateUrl: './feedback-input.component.html',
    styleUrl: './feedback-input.component.scss'
})
export class FeedbackInputComponent implements OnInit {

    protected _feedback?: OrderFeedbackResponse;

    public orderId = input<number>();
    public executorId = input<number>();
    public categoryId = input<number>();
    public canEdit = input<boolean>(false);

    protected isReadonlyMode = signal<boolean>(false);

    public isHistoryView = input<boolean>(false);

    feedbackForm!: FormGroup;

    @Input()
    set feedback(value: OrderFeedbackResponse | undefined) {
        this._feedback = value;
        if (this.feedbackForm) {
            this.patchForm(value);
        }
    }

    protected readonly toastHelper = inject(ToastHelper);
    protected readonly orderService = inject(OrderService);
    protected readonly errorHandlerService = inject(ErrorHandlerService);
    protected readonly formBuilder = inject(FormBuilder);

    ngOnInit(): void {
        this.initForm();
        if (this._feedback) {
            this.patchForm(this._feedback);
        }
    }

    private initForm(): void {
        this.feedbackForm = this.formBuilder.group({
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

    deleteFeedback() {
        if (!this._feedback || !this.orderId()) return;

        this.orderService.deleteOrderFeedback(this.orderId()!, this._feedback.feedbackId).subscribe({
            next: () => {
                this.toastHelper.showSuccess('common.success', 'feedback.feedback_deleted');
                this._feedback = undefined;
                this.feedbackForm.reset();
            },
            error: (error: HttpErrorResponse) => this.errorHandlerService.handleApiError(error)
        });
    }

    onSubmit() {
        if (this.feedbackForm.valid) {
            const createdFeedbackRequest = this.feedbackForm.value as CreateFeedbackRequest;
            createdFeedbackRequest.executorId = this.executorId()!;
            createdFeedbackRequest.categoryId = this.categoryId()!;
            const orderId = this.orderId()!;
            const feedbackId = this._feedback?.feedbackId;
            if (feedbackId) {
                this.orderService.updateOrderFeedback(orderId, feedbackId, createdFeedbackRequest).subscribe({
                    next: () => {
                        this.toastHelper.showSuccess('common.success', 'feedback.feedback_updated');
                    },
                    error: (error: HttpErrorResponse) => this.errorHandlerService.handleApiError(error)
                });
                return;
            }

            this.orderService.createOrderFeedback(orderId, createdFeedbackRequest).subscribe({
                next: () => {
                    this.toastHelper.showSuccess('common.success', 'feedback.feedback_created');
                    window.location.reload();
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
