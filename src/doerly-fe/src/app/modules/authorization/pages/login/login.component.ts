import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../domain/auth.service';
import {LoginRequest} from '../../models/requests/login-request';
import {HttpErrorResponse} from '@angular/common/http';
import {JwtTokenHelper} from 'app/@core/helpers/jwtToken.helper';
import {TranslatePipe} from '@ngx-translate/core';
import {Checkbox} from 'primeng/checkbox';
import {Ripple} from 'primeng/ripple';
import {Card} from 'primeng/card';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {Divider} from 'primeng/divider';
import {PasswordInputComponent} from 'app/@components/password/password-input.component';
import {NgIf} from "@angular/common";
import {getError, getServersideError, isInvalid, setServerErrors} from "app/@core/helpers/input-validation-helpers";
import {ToastHelper} from 'app/@core/helpers/toast.helper';

@Component({
  selector: 'app-login',
  imports: [
    InputText,
    ReactiveFormsModule,
    TranslatePipe,
    Checkbox,
    ButtonDirective,
    Ripple,
    Card,
    RouterLink,
    Divider,
    PasswordInputComponent,
    NgIf,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',

})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  returnUrl!: string;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private router: Router,
              private route: ActivatedRoute,
              private tokenHelper: JwtTokenHelper,
              private toastHelper: ToastHelper,
  ) {
  }

  ngOnInit() {
    this.initForm();
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  initForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }

  login(): void {
    const request = this.loginForm.value as LoginRequest;
    const rememberMe = this.loginForm.get('rememberMe')!.value;
    console.warn(`Remember me: ${rememberMe}`);

    this.authService.login(request).subscribe({
      next: (value) => {
        this.tokenHelper.setToken(value.value!.accessToken, rememberMe);
        this.router.navigate([this.returnUrl]);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.toastHelper.showApiError(error, 'auth.incorrect_credentials');
        } else if (error.status === 400) {
          const errors = error.error.errors;
          setServerErrors(this.loginForm, errors);
        } else {
          this.toastHelper.showApiError(error, 'common.error_occurred');
        }
      }
    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}
