import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Notification } from './notification.service';

export enum NotificationType {
  Message = 0,
  Order = 1,
  System = 2,
  Task = 3,
  Comment = 4,
  Like = 5,
  Follow = 6,
  Mention = 7
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
          this.router.navigate(['/orders', data.orderId]);
          break;

        case NotificationType.Task:
          this.router.navigate(['/tasks', data.taskId]);
          break;

        case NotificationType.Comment:
          this.router.navigate(['/posts', data.postId], {
            fragment: `comment-${data.commentId}`
          });
          break;

        case NotificationType.Like:
          this.router.navigate(['/posts', data.postId]);
          break;

        case NotificationType.Follow:
          this.router.navigate(['/profile', data.userId]);
          break;

        case NotificationType.Mention:
          if (data.postId) {
            this.router.navigate(['/posts', data.postId]);
          } else if (data.commentId) {
            this.router.navigate(['/posts', data.postId], {
              fragment: `comment-${data.commentId}`
            });
          }
          break;

        case NotificationType.System:
          // System notifications usually don't have a specific page to navigate to
          break;
      }
    } catch (error) {
      console.error('Error parsing notification data:', error);
    }
  }
}
