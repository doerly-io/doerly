import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "primeng/button";
import {Card} from "primeng/card";
import {InputText} from "primeng/inputtext";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Ripple} from "primeng/ripple";
import {Router, RouterLink} from "@angular/router";
import {TranslatePipe} from "@ngx-translate/core";
import {AuthService} from '../../domain/auth.service';
import {HttpErrorResponse, HttpStatusCode} from '@angular/common/http';
import {RegisterRequest} from '../../models/requests/register-request';
import {Divider} from 'primeng/divider';
import {NgIf} from "@angular/common";
import {getError, getServersideError, isInvalid} from 'app/@core/helpers/input-validation-helpers';
import {PasswordInputComponent} from 'app/@components/password/password-input.component';
import {ToastHelper} from 'app/@core/helpers/toast.helper';
import {BaseApiResponse} from '../../../../@core/models/base-api-response';

@Component({
  selector: 'app-register',
  imports: [
    ButtonDirective,
    Card,
    InputText,
    ReactiveFormsModule,
    Ripple,
    RouterLink,
    TranslatePipe,
    Divider,
    PasswordInputComponent,
    NgIf
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private readonly toastHelper: ToastHelper,
              private router: Router,
  ) {
  }

  ngOnInit() {
    this.initForm();
  }

  initForm(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
    });
  }

  register(): void {
    const request = this.registerForm.value as RegisterRequest;

    this.authService.register(request).subscribe({
      next: (value) => {
        this.toastHelper.showSuccess('auth.register_success', 'auth.check_email');
        this.router.navigate(['auth/login']);
      },
      error: (error: HttpErrorResponse) => {
        if (error.error as BaseApiResponse && error.status === HttpStatusCode.Conflict) {
          this.toastHelper.showError(error.error.errorMessage, 'auth.email_exists');
        }
      }
    });
  }

  protected readonly getServersideError = getServersideError;
  protected readonly isInvalid = isInvalid;
  protected readonly getError = getError;
}
