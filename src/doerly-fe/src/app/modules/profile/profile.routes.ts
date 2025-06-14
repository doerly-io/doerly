import {Routes} from '@angular/router';
import {ProfileComponent} from './pages/profile/profile.component';
import {authGuard} from 'app/@core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    canActivate: [authGuard]

  },
  {
    path: ':userId',
    component: ProfileComponent

  }
];
