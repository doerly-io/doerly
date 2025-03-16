import { Component } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { OrdersListComponent } from '../orders-list/orders-list.component';

@Component({
  selector: 'app-my-orders-tabs',
  imports: [
    TabsModule,
    OrdersListComponent
  ],
  templateUrl: './my-orders-tabs.component.html',
  styleUrl: './my-orders-tabs.component.scss'
})
export class MyOrdersTabsComponent {
  myUserId = 1;
}
