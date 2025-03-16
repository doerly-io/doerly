import {Routes} from '@angular/router';
import {authGuard} from './@core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./modules/home/home/home.component').then(m => m.HomeComponent),
    canActivate: [authGuard]
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/authorization/authorization.routes').then(m => m.routes)
  },
  {
    path: 'ordering',
    loadChildren: () => import('./modules/order/ordering.routes').then(m => m.routes),
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: ''//ToDo: 404 page
  }
];
