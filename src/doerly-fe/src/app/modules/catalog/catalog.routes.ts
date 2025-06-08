import { Routes } from '@angular/router';
import { CatalogListComponent } from './pages/catalog-list/catalog-list.component';

export const CATALOG_ROUTES: Routes = [
  {
    path: '',
    component: CatalogListComponent
  },
  {
    path: ':categoryId',
    component: CatalogListComponent
  }
]; 