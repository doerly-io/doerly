import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "primeng/button";
import {Card} from "primeng/card";
import {Checkbox} from "primeng/checkbox";
import {InputText} from "primeng/inputtext";
import {NgIf} from "@angular/common";
import {PasswordDirective} from "primeng/password";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Ripple} from "primeng/ripple";
import {Router, RouterLink} from "@angular/router";
import {TranslatePipe} from "@ngx-translate/core";
import {AuthService} from '../../domain/auth.service';
import {JwtTokenHelper} from '../../../../@core/helpers/jwtToken.helper';
import {LoginRequest} from '../../models/requests/login-request';
import {HttpErrorResponse} from '@angular/common/http';
import {RegisterRequest} from '../../models/requests/register-request';
import {Divider} from 'primeng/divider';

@Component({
  selector: 'app-register',
  imports: [
    ButtonDirective,
    Card,
    Checkbox,
    InputText,
    NgIf,
    PasswordDirective,
    ReactiveFormsModule,
    Ripple,
    RouterLink,
    TranslatePipe,
    Divider
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  isPasswordVisible: boolean = false;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private router: Router,
  ) {
  }

  ngOnInit() {
    this.initForm();
  }

  initForm(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  togglePasswordVisibility = () => this.isPasswordVisible = !this.isPasswordVisible;

  register(): void {
    const request = this.registerForm.value as RegisterRequest;

    this.authService.register(request).subscribe({
      next: (value) => {
        if (value.isSuccess) {
          this.router.navigate(['/login']);
        }
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 401) {
          console.error('Invalid email or password');
        }
      }
    });
  }

}
