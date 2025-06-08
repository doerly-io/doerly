import { Component, Input } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { TabsModule } from 'primeng/tabs';
import { CatalogListComponent } from "../catalog-list/catalog-list.component";
import { OrdersListComponent } from "../orders-list/orders-list.component";

@Component({
  selector: 'app-catalog-tabs',
  imports: [
    TabsModule,
    TranslatePipe,
    CatalogListComponent,
    OrdersListComponent
],
  templateUrl: './catalog-tabs.component.html',
  styleUrl: './catalog-tabs.component.scss'
})
export class CatalogTabsComponent {
    @Input() tab: number = 0;

}
