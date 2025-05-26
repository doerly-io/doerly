import { Routes } from "@angular/router";
import { OrderDetailsComponent } from "./pages/order-details/order-details.component";
import { OrderingTabsComponent } from "./pages/ordering-tabs/ordering-tabs.component";
import { ExecutionProposalDetailsComponent } from "./pages/execution-proposal-details/execution-proposal-details.component";
import { EditOrderComponent } from "./pages/edit-order/edit-order.component";
import { EditExecutionProposalComponent } from "./pages/edit-execution-proposal/edit-execution-proposal.component";

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
    component: EditOrderComponent
  },
  {
    path: 'edit-order/:id',
    component: EditOrderComponent
  },
  {
    path: "send-execution-proposal",
    component: EditExecutionProposalComponent
  },
  {
    path: 'edit-execution-proposal/:id',
    component: EditExecutionProposalComponent
  },
  {
    path: "**",
    component: OrderingTabsComponent
  }
]