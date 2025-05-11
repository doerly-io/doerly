import { Component } from '@angular/core';
import {ConversationListComponent} from '../conversation-list/conversation-list.component';
import {ChatWindowComponent} from '../chat-window/chat-window.component';

@Component({
  selector: 'app-communication',
  imports: [
    ConversationListComponent,
    ChatWindowComponent
  ],
  templateUrl: './communication.component.html',
  styleUrl: './communication.component.scss'
})
export class CommunicationComponent {

}
