import { Routes } from "@angular/router";
import { OrdersListComponent } from "./pages/orders-list/orders-list.component";
import { OrderDetailsComponent } from "./pages/order-details/order-details.component";
import { ExecutionProposalsListComponent } from "./pages/execution-proposals-list/execution-proposals-list.component";
import { OrderingTabsComponent } from "./pages/ordering-tabs/ordering-tabs.component";

export const routes: Routes = [
  {
    path: 'orders',
    component: OrdersListComponent
  },
  { 
    path: 'order/:id', 
    component: OrderDetailsComponent 
  },
  {
    path: 'execution-proposals',
    component: ExecutionProposalsListComponent
  },
  {
    path: "**",
    component: OrderingTabsComponent
  }
]