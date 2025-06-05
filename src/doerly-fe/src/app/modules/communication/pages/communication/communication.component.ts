import {Component, HostListener, signal} from '@angular/core';
import {ConversationListComponent} from '../conversation-list/conversation-list.component';
import {ChatWindowComponent} from '../chat-window/chat-window.component';
import {ButtonModule} from 'primeng/button';
import {NgClass, NgIf} from '@angular/common';
import {TranslateModule} from '@ngx-translate/core';

@Component({
  selector: 'app-communication',
  standalone: true,
  imports: [
    ConversationListComponent,
    ChatWindowComponent,
    ButtonModule,
    NgIf,
    NgClass,
    TranslateModule
  ],
  templateUrl: './communication.component.html',
  styleUrl: './communication.component.scss'
})
export class CommunicationComponent {
  protected readonly selectedConversationId = signal<number | undefined>(undefined);
  protected readonly isMobileView = signal<boolean>(false);
  private readonly MOBILE_BREAKPOINT = 768;

  constructor() {
    this.checkScreenSize();
  }

  @HostListener('window:resize')
  onResize() {
    this.checkScreenSize();
  }

  private checkScreenSize() {
    this.isMobileView.set(window.innerWidth <= this.MOBILE_BREAKPOINT);
  }

  onConversationSelected(conversationId: number) {
    this.selectedConversationId.set(conversationId);
  }

  onBackClick() {
    this.selectedConversationId.set(undefined);
  }
}
