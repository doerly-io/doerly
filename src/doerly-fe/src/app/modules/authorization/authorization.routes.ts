import {Routes} from '@angular/router';
import {LoginComponent} from './pages/login/login.component';
import {RegisterComponent} from './pages/register/register.component';
import {RequestPasswordResetComponent} from './pages/request-password-reset/request-password-reset.component';
import {PasswordResetComponent} from './pages/password-reset/password-reset.component';

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
  }
]
