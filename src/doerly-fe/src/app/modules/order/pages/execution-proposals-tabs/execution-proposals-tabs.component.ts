import { Component, Input } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { ExecutionProposalsListComponent } from '../execution-proposals-list/execution-proposals-list.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-execution-proposals-tabs',
  imports: [
    TabsModule,
    ExecutionProposalsListComponent,
    TranslatePipe,
  ],
  templateUrl: './execution-proposals-tabs.component.html',
  styleUrl: './execution-proposals-tabs.component.scss'
})
export class ExecutionProposalsTabsComponent {
  @Input() tab: number = 0;

  profileId: number = 0;
}
