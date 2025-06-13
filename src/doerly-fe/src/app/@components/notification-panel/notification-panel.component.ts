import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../../@core/services/notification.service';
import { NotificationPanelService } from '../../@core/services/notification-panel.service';
import { NotificationNavigationService } from '../../@core/services/notification-navigation.service';
import { Subscription } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-notification-panel',
  standalone: true,
  imports: [CommonModule, ButtonModule, CardModule, ScrollPanelModule, RippleModule],
  template: `
    <div class="notification-panel" *ngIf="isVisible$ | async">
      <p-card styleClass="notification-card">
        <ng-template pTemplate="header">
          <div class="panel-header">
            <h3>Notifications</h3>
            <p-button 
              *ngIf="hasUnreadNotifications"
              label="Mark all as read"
              icon="pi pi-check"
              (click)="markAllAsRead()"
              [text]="true"
              [rounded]="true"
              [outlined]="true"
            ></p-button>
          </div>
        </ng-template>

        <p-scrollPanel [style]="{width: '100%', height: '400px'}">
          <div class="panel-content">
            <div *ngIf="notifications.length === 0" class="no-notifications">
              <i class="pi pi-bell" style="font-size: 2rem"></i>
              <p>No notifications</p>
            </div>
            
            <div *ngFor="let notification of notifications" 
                 class="notification-item"
                 [class.unread]="!notification.isRead"
                 (click)="handleNotificationClick(notification)"
                 pRipple>
              <div class="notification-content">
                <div class="notification-header">
                  <i [class]="getNotificationIcon(notification.type)" class="notification-icon"></i>
                  <h4>{{ notification.title }}</h4>
                </div>
                <p>{{ notification.message }}</p>
                <small>{{ notification.createdAt | date:'short' }}</small>
              </div>
            </div>
          </div>
        </p-scrollPanel>
      </p-card>
    </div>
  `,
  styles: [`
    .notification-panel {
      position: absolute;
      top: 100%;
      right: 0;
      width: 350px;
      z-index: 1000;
      margin-top: 0.5rem;
    }

    :host ::ng-deep {
      .notification-card {
        .p-card {
          background: var(--p-surface-ground);
          border: 1px solid var(--p-surface-700);
        }

        .p-card-header {
          padding: 1rem;
          border-bottom: 1px solid var(--p-surface-700);
        }

        .p-card-body {
          padding: 0;
        }

        .p-card-content {
          padding: 0;
        }
      }

      .p-scrollpanel {
        .p-scrollpanel-wrapper {
          border-right: 0;
        }

        .p-scrollpanel-bar {
          background: var(--p-surface-700);
        }
      }
    }

    .panel-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .panel-header h3 {
      margin: 0;
      font-size: 1rem;
      color: var(--p-surface-400);
    }

    .panel-content {
      .notification-item {
        padding: 1rem;
        border-bottom: 1px solid var(--p-surface-700);
        cursor: pointer;
        transition: background-color 0.2s;

        &:hover {
          background-color: var(--p-surface-700);
        }

        &.unread {
          background-color: var(--p-surface-700);
        }

        .notification-content {
          .notification-header {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin-bottom: 0.5rem;

            .notification-icon {
              font-size: 1rem;
              color: var(--p-primary-color);
            }
          }

          h4 {
            margin: 0;
            font-size: 0.875rem;
            color: var(--p-surface-400);
          }

          p {
            margin: 0 0 0.5rem 0;
            font-size: 0.813rem;
            color: var(--p-surface-400);
          }

          small {
            color: var(--p-surface-400);
            font-size: 0.75rem;
            opacity: 0.7;
          }
        }
      }
    }

    .no-notifications {
      padding: 2rem;
      text-align: center;
      color: var(--p-surface-400);

      i {
        margin-bottom: 1rem;
        opacity: 0.5;
      }

      p {
        margin: 0;
        font-size: 0.875rem;
      }
    }

    @media screen and (max-width: 576px) {
      .notification-panel {
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        width: 100%;
        height: 100%;
        margin: 0;
        border-radius: 0;
      }

      :host ::ng-deep {
        .notification-card {
          height: 100%;
          border-radius: 0;

          .p-card-body {
            height: calc(100% - 4rem);
          }
        }

        .p-scrollpanel {
          height: 100%;
        }
      }
    }
  `]
})
export class NotificationPanelComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  private subscriptions: Subscription[] = [];
  isVisible$;

  constructor(
    private notificationService: NotificationService,
    private panelService: NotificationPanelService,
    private navigationService: NotificationNavigationService
  ) {
    this.isVisible$ = this.panelService.isVisible$;
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.notificationService.notifications$.subscribe(notifications => {
        this.notifications = notifications;
      })
    );

    this.loadNotifications();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  get hasUnreadNotifications(): boolean {
    return this.notifications.some(n => !n.isRead);
  }

  loadNotifications(): void {
    this.notificationService.getNotifications().subscribe();
  }

  handleNotificationClick(notification: Notification): void {
    this.markAsRead([notification.id]);
    this.navigationService.navigateToNotificationSource(notification);
    this.panelService.close();
  }

  markAsRead(notificationIds: number[]): void {
    this.notificationService.markAsRead(notificationIds).subscribe();
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead().subscribe();
  }

  getNotificationIcon(type: number): string {
    switch (type) {
      case 0: return 'pi pi-envelope'; // Message
      case 1: return 'pi pi-shopping-cart'; // Order
      case 2: return 'pi pi-info-circle'; // System
      case 3: return 'pi pi-check-square'; // Task
      case 4: return 'pi pi-comments'; // Comment
      case 5: return 'pi pi-heart'; // Like
      case 6: return 'pi pi-user-plus'; // Follow
      case 7: return 'pi pi-at'; // Mention
      default: return 'pi pi-bell';
    }
  }
} 