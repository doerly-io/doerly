import { Component, Input } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { OrdersListComponent } from '../orders-list/orders-list.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-my-orders-tabs',
  imports: [
    TabsModule,
    OrdersListComponent,
    TranslatePipe
  ],
  templateUrl: './my-orders-tabs.component.html',
  styleUrl: './my-orders-tabs.component.scss'
})
export class MyOrdersTabsComponent {
  @Input() tab: number = 0;
  profileId: number = 1;
}
