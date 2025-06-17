import {Routes} from '@angular/router';
import {LoginComponent} from 'app/modules/authorization/pages/login/login.component';
import {RegisterComponent} from 'app/modules/authorization/pages/register/register.component';
import {RequestPasswordResetComponent} from 'app/modules/authorization/pages/request-password-reset/request-password-reset.component';
import {PasswordResetComponent} from 'app/modules/authorization/pages/password-reset/password-reset.component';
import {EmailVerificationComponent} from 'app/modules/authorization/pages/email-verification/email-verification.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'request-password-reset',
    component: RequestPasswordResetComponent
  },
  {
    path: 'password-reset',
    component: PasswordResetComponent
  },
  {
    //token and email are query params
    path: 'email-verification',
    component: EmailVerificationComponent
  }
]
