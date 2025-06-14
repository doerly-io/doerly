import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../@core/services/notification.service';
import { NotificationPanelService } from '../../@core/services/notification-panel.service';
import { Subscription } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { BadgeModule } from 'primeng/badge';

@Component({
  selector: 'app-notification-badge',
  standalone: true,
  imports: [CommonModule, ButtonModule, BadgeModule],
  template: `
    <p-button
      icon="pi pi-bell"
      (click)="toggleNotifications()"
      [text]="true"
      [rounded]="true"
      [outlined]="true"
      [badge]="unreadCount > 0 ? unreadCount.toString() : ''"
      [badgeClass]="'notification-badge'"
      class="notification-button"
    ></p-button>
  `,
  styles: [`
    :host ::ng-deep {
      .notification-button {
        .p-button {
          background: var(--p-surface-ground);
          border-color: var(--p-surface-700);
          color: var(--p-surface-400);

          &:hover {
            background: var(--p-surface-700);
            border-color: var(--p-surface-700);
            color: var(--p-surface-400);
          }
        }
      }

      .notification-badge {
        background: var(--p-primary-color) !important;
        color: white !important;
        font-size: 0.75rem !important;
        min-width: 1.5rem !important;
        height: 1.5rem !important;
        line-height: 1.5rem !important;
        border-radius: 50% !important;
        padding: 0 !important;
        margin-top: -0.5rem !important;
        margin-right: -0.5rem !important;
      }
    }
  `]
})
export class NotificationBadgeComponent implements OnInit, OnDestroy {
  unreadCount = 0;
  private subscriptions: Subscription[] = [];

  constructor(
    private notificationService: NotificationService,
    private panelService: NotificationPanelService
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.notificationService.unreadCount$.subscribe(count => {
        this.unreadCount = count;
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  toggleNotifications(): void {
    this.panelService.toggle();
  }
}
