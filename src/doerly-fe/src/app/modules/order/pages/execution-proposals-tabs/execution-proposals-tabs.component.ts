import { Component } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { ExecutionProposalsListComponent } from '../execution-proposals-list/execution-proposals-list.component';

@Component({
  selector: 'app-execution-proposals-tabs',
  imports: [
    TabsModule,
    ExecutionProposalsListComponent
  ],
  templateUrl: './execution-proposals-tabs.component.html',
  styleUrl: './execution-proposals-tabs.component.scss'
})
export class ExecutionProposalsTabsComponent {
  myUserId = 1;
}
