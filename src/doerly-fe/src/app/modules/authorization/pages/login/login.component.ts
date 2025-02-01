import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {PasswordDirective} from 'primeng/password';
import {AuthService} from '../../domain/auth.service';
import {LoginRequest} from '../../models/requests/login-request';
import {HttpErrorResponse} from '@angular/common/http';
import {JwtTokenHelper} from '../../../../@core/helpers/jwtToken.helper';
import {NgIf} from '@angular/common';
import {TranslatePipe} from '@ngx-translate/core';
import {Checkbox} from 'primeng/checkbox';
import {Ripple} from 'primeng/ripple';
import {Card} from 'primeng/card';
import {RouterLink} from '@angular/router';
import {Divider} from 'primeng/divider';

@Component({
  selector: 'app-login',
  imports: [
    InputText,
    ReactiveFormsModule,
    PasswordDirective,
    NgIf,
    TranslatePipe,
    Checkbox,
    ButtonDirective,
    Ripple,
    Card,
    RouterLink,
    Divider
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  isPasswordVisible: boolean = false;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private tokenHelper: JwtTokenHelper
  ) {
  }

  ngOnInit() {
    this.initForm();
  }

  initForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  togglePasswordVisibility = () => this.isPasswordVisible = !this.isPasswordVisible;

  login(): void {
    const request = this.loginForm.value as LoginRequest;

    this.authService.login(request).subscribe({
      next: (value) => {
        this.tokenHelper.setToken(value.value!.accessToken);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 401) {
          console.error('Invalid email or password');
        }
      }
    });
  }
}
