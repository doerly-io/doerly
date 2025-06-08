import {Routes} from '@angular/router';
import {CommunicationComponent} from './pages/communication/communication.component';
import {ChatWindowComponent} from './pages/chat-window/chat-window.component';

export const routes: Routes = [
  {
    path: '',
    component: CommunicationComponent
  },
  {
    path: 'conversation/:id',
    component: ChatWindowComponent
  }
]
