import { Routes } from '@angular/router';
import { CatalogListComponent } from './pages/catalog-list/catalog-list.component';
import { CatalogTabsComponent } from './pages/catalog-tabs/catalog-tabs.component';

export const CATALOG_ROUTES: Routes = [
  {
    path: '',
    component: CatalogTabsComponent,
  },
  {
    path: ':categoryId',
    component: CatalogTabsComponent
  }
]; 
