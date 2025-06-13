import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../../@core/services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notification-container">
      <div class="notification-header">
        <h3>Notifications</h3>
        <div class="notification-actions">
          <button (click)="markAllAsRead()" *ngIf="hasUnreadNotifications">
            Mark all as read
          </button>
        </div>
      </div>

      <div class="notification-list">
        <div *ngIf="notifications.length === 0" class="no-notifications">
          No notifications
        </div>
        
        <div *ngFor="let notification of notifications" 
             class="notification-item"
             [class.unread]="!notification.isRead"
             (click)="markAsRead([notification.id])">
          <div class="notification-content">
            <h4>{{ notification.title }}</h4>
            <p>{{ notification.message }}</p>
            <small>{{ notification.createdAt | date:'medium' }}</small>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .notification-container {
      width: 100%;
      max-width: 600px;
      margin: 0 auto;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .notification-header {
      padding: 16px;
      border-bottom: 1px solid #eee;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .notification-actions button {
      padding: 8px 16px;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }

    .notification-list {
      max-height: 500px;
      overflow-y: auto;
    }

    .notification-item {
      padding: 16px;
      border-bottom: 1px solid #eee;
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .notification-item:hover {
      background-color: #f8f9fa;
    }

    .notification-item.unread {
      background-color: #f0f7ff;
    }

    .notification-content h4 {
      margin: 0 0 8px 0;
      color: #333;
    }

    .notification-content p {
      margin: 0 0 8px 0;
      color: #666;
    }

    .notification-content small {
      color: #999;
    }

    .no-notifications {
      padding: 32px;
      text-align: center;
      color: #666;
    }
  `]
})
export class NotificationComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  private subscriptions: Subscription[] = [];

  constructor(private notificationService: NotificationService) {}

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

  markAsRead(notificationIds: number[]): void {
    this.notificationService.markAsRead(notificationIds).subscribe();
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead().subscribe();
  }
} 