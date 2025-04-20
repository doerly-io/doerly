import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../domain/auth.service';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ButtonDirective} from 'primeng/button';
import {Card} from 'primeng/card';
import {Ripple} from 'primeng/ripple';
import {TranslatePipe} from '@ngx-translate/core';
import {PasswordInputComponent} from 'app/@components/password/password-input.component';
import {NgIf} from '@angular/common';
import {PasswordResetRequest} from '../../models/requests/password-reset-request';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'app-password-reset',
  imports: [
    ButtonDirective,
    Card,
    Ripple,
    TranslatePipe,
    ReactiveFormsModule,
    PasswordInputComponent,
    NgIf,
  ],
  templateUrl: './password-reset.component.html',
  styleUrl: './password-reset.component.scss'
})
export class PasswordResetComponent implements OnInit {

  passwordResetForm!: FormGroup;
  token!: string;
  email!: string;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private formBuilder: FormBuilder,
              private authorizationService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
      this.email = params['email'];
      this.passwordResetForm = this.formBuilder.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
      }, { validator: this.passwordMatchValidator });

      if (!this.token) {
        this.router.navigate(['/404']);
      }
    });
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('password')!;
    const confirmPassword = formGroup.get('confirmPassword')!;
    const isValidPassword = password.valid && confirmPassword.valid
    if (!isValidPassword) //skip match validation if one of the password is invalid
      return true;
    return password.value === confirmPassword.value ? null : {mismatch: true};
  }


  resetPassword() {
    const password = this.passwordResetForm.get('password')!.value;
    const request : PasswordResetRequest = {
      password: password,
      token: this.token,
      email: this.email
    };
    this.authorizationService.resetPassword(request).subscribe({
      next: (value) => {

        this.router.navigate(['/login'], {relativeTo: this.route});
      },
      error: (error) => {
        console.error(error);
      }
    });

  }


}
