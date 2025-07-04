import {Component, OnInit, OnDestroy, ViewChild} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../../@core/services/notification.service';
import { NotificationPanelService } from '../../@core/services/notification-panel.service';
import { NotificationNavigationService } from '../../@core/services/notification-navigation.service';
import { Subscription } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { RippleModule } from 'primeng/ripple';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { NotificationType } from '../../@core/enums/notification-type.enum';
import {EOrderStatus} from '../../modules/order/domain/enums/order-status';
import {EExecutionProposalStatus} from '../../modules/order/domain/enums/execution-proposal-status';
import {OverlayPanelModule} from 'primeng/overlaypanel';
import {Popover} from 'primeng/popover';

@Component({
  selector: 'app-notification-panel',
  standalone: true,
  imports: [CommonModule, ButtonModule, CardModule, ScrollPanelModule, RippleModule, TranslatePipe, OverlayPanelModule, Popover],
  template: `
    <div class="notification-panel">
      <p-popover #op [dismissable]="true">
        <p-card styleClass="notification-card">
          <ng-template pTemplate="header">
            <div class="panel-header">
              <h3>{{ 'notification.title' | translate }}</h3>
              <p-button
                *ngIf="hasUnreadNotifications"
                [label]="'notification.mark_all_read' | translate"
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
                <p>{{ 'notification.no_notifications' | translate }}</p>
              </div>

              <div *ngFor="let notification of notifications"
                   class="notification-item"
                   [class.unread]="!notification.isRead"
                   (click)="handleNotificationClick(notification)"
                   pRipple>
                <div class="notification-content">
                  <div class="notification-header">
                    <i [class]="getNotificationIcon(notification.type)" class="notification-icon"></i>
                    <h4>{{ getNotificationTitle(notification) }}</h4>
                  </div>
                  <p>{{ getNotificationMessage(notification) }}</p>
                  <small>{{ notification.timestamp | date: 'dd.MM.yyyy HH:mm' }}</small>
                </div>
              </div>
            </div>
          </p-scrollPanel>
        </p-card>
      </p-popover>
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

      .p-button {
        font-size: 0.75rem;
        padding: 0.25rem 0.5rem;
        line-height: 1.5;
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
        border-bottom: 1px solid var(--p-surface-400);
        cursor: pointer;
        transition: background-color 0.2s;

        &:hover {
          background-color: var(--p-primary-300);
        }

        &.unread {
          background-color: var(--p-surface-200);
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

    :host-context(.my-app-dark) {
      .panel-content {
        .notification-item {
          background-color: var(--p-surface-800);
          border-bottom-color: var(--p-surface-700);

          &:hover {
            background-color: var(--p-surface-700);
          }

          &.unread {
            background-color: var(--p-surface-700);
          }

          .notification-content {
            h4 {
              color: var(--p-surface-0);
            }

            p {
              color: var(--p-surface-300);
            }

            small {
              color: var(--p-surface-400);
            }
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
            padding: 0 !important;
            height: calc(100% - 4rem);
          }
        }

        .p-scrollpanel {
          height: 100%;
        }

        .p-button {
          font-size: 0.75rem;
          padding: 0.25rem 0.5rem;
          line-height: 1.5;
        }
      }
    }
  `]
})
export class NotificationPanelComponent implements OnInit, OnDestroy {
  @ViewChild('op') op!: Popover;

  notifications: Notification[] = [];
  private subscriptions: Subscription[] = [];

  constructor(
    private notificationService: NotificationService,
    private panelService: NotificationPanelService,
    private navigationService: NotificationNavigationService,
    private translateService: TranslateService
  ) {
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.notificationService.notifications$.subscribe(notifications => {
        this.notifications = notifications;
      }),
      this.panelService.toggle$.subscribe(event => {
        if (this.op) {
          this.op.toggle(event);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  get hasUnreadNotifications(): boolean {
    return this.notifications.some(n => !n.isRead);
  }

  handleNotificationClick(notification: Notification): void {
    this.markAsRead([notification.id]);
    this.navigationService.navigateToNotificationSource(notification);
    if (this.op) {
      this.op.hide();
    }
  }

  markAsRead(notificationIds: number[]): void {
    this.notificationService.markAsRead(notificationIds).subscribe();
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead().subscribe();
  }

  getNotificationTitle(notification: Notification): string {
    return this.translateService.instant(`notification.types.${NotificationType[notification.type]}`);
  }

  getNotificationMessage(notification: Notification): string {
    try {
      const data = notification.data ? JSON.parse(notification.data) : {};
      console.log(data);

      if (notification.type === NotificationType.Order && notification.message === 'Notification.Order.StatusChanged') {
        if (data.orderStatus) {
          data.orderStatus = this.translateService.instant(`ordering.order_statuses.${EOrderStatus[data.orderStatus]}`);
        }

        let translated = '';
        this.translateService.get('Notification.Order.StatusChanged', {id: data.orderId, status: data.orderStatus}).subscribe(translate => {
          translated = translate;
        });
        return translated;
      }

      if (notification.type === NotificationType.ExecutionProposal && notification.message === 'Notification.ExecutionProposal.StatusChanged') {
        if (data.executionProposalStatus) {
          data.executionProposalStatus = this.translateService.instant(`ordering.proposal_statuses.${EExecutionProposalStatus[data.executionProposalStatus]}`);
        }

        let translated = '';
        this.translateService.get('Notification.ExecutionProposal.StatusChanged', {id: data.executionProposalId, status: data.executionProposalStatus}).subscribe(translate => {
          translated = translate;
        });
        return translated;
      }

      if (notification.type === NotificationType.Message && notification.message === 'Notification.Message.NewMessage') {
        let translated = '';
        this.translateService.get('Notification.Message.NewMessage', {senderName: data.senderName}).subscribe(translate => {
          translated = translate;
        });
        return translated;
      }

      const messageKey = notification.message;
      return this.translateService.instant(messageKey, data);
    } catch (e) {
      console.error('Error parsing notification data:', e);
      return notification.message;
    }
  }

  getNotificationIcon(type: NotificationType): string {
    switch (type) {
      case NotificationType.Message:
        return 'pi pi-envelope';
      case NotificationType.Order:
        return 'pi pi-shopping-cart';
      default:
        return 'pi pi-envelope';
    }
  }
}
