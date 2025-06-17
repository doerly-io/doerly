import {Routes} from '@angular/router';
import {CommunicationComponent} from './pages/communication/communication.component';

export const routes: Routes = [
  {
    path: 'conversations',
    component: CommunicationComponent
  },
  {
    path: 'conversations/:conversationId',
    component: CommunicationComponent
  }
]
