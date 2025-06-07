import {Component, input, model} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Card} from 'primeng/card';
import {Rating} from 'primeng/rating';
import {Textarea} from 'primeng/textarea';
import {ButtonDirective} from 'primeng/button';
import {NgClass, NgIf} from '@angular/common';
import {TranslatePipe} from '@ngx-translate/core';
import {getError, getServersideError, isInvalid} from 'app/@core/helpers/input-validation-helpers';

@Component({
  selector: 'app-feedback-input',
  imports: [
    Card,
    Rating,
    ReactiveFormsModule,
    Textarea,
    ButtonDirective,
    NgIf,
    TranslatePipe,
    NgClass
  ],
  templateUrl: './feedback-input.component.html',
  styleUrl: './feedback-input.component.scss',
  standalone: true
})
export class FeedbackInputComponent {

  public orderId = input<number>();
  public isReadonly = input<boolean>(false);

  feedbackForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.feedbackForm = this.fb.group({
      rating: [null, [Validators.required]],
      comment: ['', [Validators.required, Validators.maxLength(2000)]]
    });
  }

  get rating() {
    return this.feedbackForm.get('rating');
  }

  get comment() {
    return this.feedbackForm.get('comment');
  }

  onSubmit() {
    if (this.feedbackForm.valid) {
      const feedback = {
        orderId: this.orderId,
        ...this.feedbackForm.value
      };
      // TODO: send feedback to API
      console.log('Feedback submitted:', feedback);
    } else {
      this.feedbackForm.markAllAsTouched();
    }
  }

  protected readonly getServersideError = getServersideError;
  protected readonly isInvalid = isInvalid;
  protected readonly getError = getError;
}
