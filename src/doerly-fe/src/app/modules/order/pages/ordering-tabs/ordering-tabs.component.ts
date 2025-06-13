import { Component, OnInit } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { ExecutionProposalsTabsComponent } from '../execution-proposals-tabs/execution-proposals-tabs.component';
import { OrdersListComponent } from '../orders-list/orders-list.component';
import { MyOrdersTabsComponent } from '../my-orders-tabs/my-orders-tabs.component';
import { TranslatePipe } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ordering-tabs',
  imports: [
    TabsModule,
    ExecutionProposalsTabsComponent,
    OrdersListComponent,
    MyOrdersTabsComponent,
    TranslatePipe
  ],
  templateUrl: './ordering-tabs.component.html',
  styleUrl: './ordering-tabs.component.scss'
})
export class OrderingTabsComponent implements OnInit {
  tab: number = 2;
  subTab: number = 0;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const tab = params['tab'];
      if (tab) 
        this.tab = Number(tab);
      const subTab = params['subTab'];
      if (subTab) 
        this.subTab = Number(subTab);
    });
  }
}
