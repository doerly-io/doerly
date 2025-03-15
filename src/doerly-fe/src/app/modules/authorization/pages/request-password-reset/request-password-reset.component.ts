import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../domain/auth.service';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {TranslatePipe} from '@ngx-translate/core';
import {InputText} from 'primeng/inputtext';
import {ButtonDirective} from 'primeng/button';
import {Ripple} from 'primeng/ripple';
import {Card} from 'primeng/card';
import {NgIf} from '@angular/common';
import {getError, getServersideError, isInvalid} from 'app/@core/helpers/input-validation-helpers';


@Component({
  selector: 'app-reset',
  imports: [
    TranslatePipe,
    InputText,
    ReactiveFormsModule,
    ButtonDirective,
    Ripple,
    Card,
    NgIf
  ],
  templateUrl: './request-password-reset.component.html',
  styleUrl: './request-password-reset.component.scss'
})
export class RequestPasswordResetComponent implements OnInit {
  isSubmitted = false;
  emailFormGroup!: FormGroup;

  constructor(private readonly authService: AuthService,
              private readonly formBuilder: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.emailFormGroup = this.formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  requestPasswordReset(): void {
    const email = this.emailFormGroup.get('email')?.value;

    this.authService.requestPasswordRequest(email).subscribe({
      next: (response) => {
        this.isSubmitted = true;
      },
      error: error => {
        console.error(error);
      }

    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;

}
