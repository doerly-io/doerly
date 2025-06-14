import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Notification } from './notification.service';

export enum NotificationType {
  Message = 0,
  Order = 1,
}

@Injectable({
  providedIn: 'root'
})
export class NotificationNavigationService {
  constructor(private router: Router) {}

  navigateToNotificationSource(notification: Notification): void {
    if (!notification.data) {
      return;
    }

    try {
      const data = JSON.parse(notification.data);

      switch (Number(notification.type)) {
        case NotificationType.Message:
          this.router.navigate(['/communication/conversations'], {
            queryParams: { conversationId: data.conversationId }
          });
          break;

        case NotificationType.Order:
          this.router.navigate(['/orders']);
          break;
      }
    } catch (error) {
      console.error('Error parsing notification data:', error);
    }
  }
}
