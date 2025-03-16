import { Component } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { ExecutionProposalsTabsComponent } from '../execution-proposals-tabs/execution-proposals-tabs.component';
import { OrdersListComponent } from '../orders-list/orders-list.component';
import { MyOrdersTabsComponent } from '../my-orders-tabs/my-orders-tabs.component';

@Component({
  selector: 'app-ordering-tabs',
  imports: [
    TabsModule,
    ExecutionProposalsTabsComponent,
    OrdersListComponent,
    MyOrdersTabsComponent
  ],
  templateUrl: './ordering-tabs.component.html',
  styleUrl: './ordering-tabs.component.scss'
})
export class OrderingTabsComponent {

}
