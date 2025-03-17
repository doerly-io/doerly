import { Routes } from "@angular/router";
import { OrderDetailsComponent } from "./pages/order-details/order-details.component";
import { OrderingTabsComponent } from "./pages/ordering-tabs/ordering-tabs.component";
import { CreateOrderComponent } from "./pages/create-order/create-order.component";
import { SendExecutionProposalComponent } from "./pages/send-execution-proposal/send-execution-proposal.component";
import { ExecutionProposalDetailsComponent } from "./pages/execution-proposal-details/execution-proposal-details.component";

export const routes: Routes = [
  { 
    path: 'order/:id', 
    component: OrderDetailsComponent 
  },
  {
    path: 'execution-proposal/:id',
    component: ExecutionProposalDetailsComponent
  },
  {
    path: 'create-order',
    component: CreateOrderComponent
  },
  {
    path: "send-execution-proposal",
    component: SendExecutionProposalComponent
  },
  {
    path: "**",
    component: OrderingTabsComponent
  }
]