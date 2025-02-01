import {Routes} from '@angular/router';
import {AuthGuard} from './@core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/authorization/authorization.routes').then(m => m.routes)
  },
  {
    path: '**',
    redirectTo: ''//ToDo: 404 page
  }
];
