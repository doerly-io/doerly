import {Routes} from '@angular/router';
import {authGuard} from './@core/guards/auth.guard';
import {NotFoundPageComponent} from 'app/shared-pages/not-found-page/not-found-page.component';

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
    path: 'profile',
    loadChildren: () => import('./modules/profile/profile.routes').then(m => m.routes),
    canActivate: [authGuard]
  },
  {
    path: 'communication',
    loadChildren: () => import('./modules/communication/communication.routes').then(m => m.routes),
    canActivate: [authGuard]
  },
  {
    path: 'ordering',
    loadChildren: () => import('./modules/order/ordering.routes').then(m => m.routes),
    canActivate: [authGuard]
  },
  {
    path: '404-page',
    component: NotFoundPageComponent
  },
  {
    path: '**',
    redirectTo: '/404-page'
  }
];
