import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ReviewService} from 'app/@core/services/review.service';
import {UpsertReview} from 'app/@core/models/review/add-review.model';
import {Card} from 'primeng/card';
import {ButtonDirective} from 'primeng/button';
import {Rating} from 'primeng/rating';
import {NgIf} from '@angular/common';
import {Textarea} from 'primeng/textarea';
import {TranslatePipe} from '@ngx-translate/core';
import {getError, isInvalid} from 'app/@core/helpers/input-validation-helpers';

@Component({
  selector: 'app-review-input',
  imports: [
    Card,
    ButtonDirective,
    ReactiveFormsModule,
    Rating,
    NgIf,
    Textarea,
    TranslatePipe
  ],
  templateUrl: './review-input.component.html',
  styleUrl: './review-input.component.scss'
})
export class ReviewInputComponent {
  private readonly reviewService = inject(ReviewService);
  private readonly fb = inject(FormBuilder);

  reviewForm: FormGroup = this.fb.group({
    rating: [0, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(4000)]],
  });

  addReview(): void {
    if (this.reviewForm.invalid) return;
    const review: UpsertReview = this.reviewForm.value;
    this.reviewService.addReview(1, review).subscribe({
      next: () => {
        console.log('Review added successfully');
        this.reviewForm.reset({rating: 0, comment: ''});
      },
      error: (error) => {
        console.error('Error adding review:', error);
      }
    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
}
