import {Routes} from '@angular/router';
import {LoginComponent} from './pages/login/login.component';
import {RegisterComponent} from './pages/register/register.component';
import {RequestPasswordResetComponent} from './pages/request-password-reset/request-password-reset.component';

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
    path: 'reset-password',
    component: RequestPasswordResetComponent
  },
  // {
  //   path: 'password-reset/:token',
  //
  //
  // }
]
