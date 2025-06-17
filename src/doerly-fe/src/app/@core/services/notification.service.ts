import {inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {HttpTransportType, HubConnection} from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationType } from '../enums/notification-type.enum';
import * as signalR from '@microsoft/signalr';
import {JwtTokenHelper} from '../helpers/jwtToken.helper';

export interface Notification {
  id: number;
  userId: number;
  message: string;
  type: NotificationType;
  isRead: boolean;
  timestamp: Date;
  data?: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubUrl = environment.baseApiUrl.replace(/\/api$/, '') + '/notificationhub';
  private apiUrl = environment.baseApiUrl + '/notification';

  private readonly jwtTokenHelper = inject(JwtTokenHelper);

  private hubConnection: HubConnection | undefined;
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private unreadCountSubject = new BehaviorSubject<number>(0);

  public notifications$ = this.notificationsSubject.asObservable();
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private http: HttpClient) {
    this.initializeHubConnection();
    this.loadInitialData();
  }

  private loadInitialData(): void {
    this.getUnreadCount().subscribe({
      next: (count: number) => {
        this.unreadCountSubject.next(count);
      },
      error: (err) => console.error('Error loading unread count:', err)
    });

    this.getNotifications().subscribe({
      next: (response: { items: Notification[], totalCount: number }) => {
        this.notificationsSubject.next(response.items);
      },
      error: (err) => console.error('Error loading notifications:', err)
    });
  }

  private initializeHubConnection(): void {
    const accessToken = this.jwtTokenHelper.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => accessToken,
        transport: HttpTransportType.ServerSentEvents
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      const currentNotifications = this.notificationsSubject.value;
      this.notificationsSubject.next([notification, ...currentNotifications]);

      if (!notification.isRead) {
        this.unreadCountSubject.next(this.unreadCountSubject.value + 1);
      }
    });

    this.hubConnection.on('NotificationsMarkedAsRead', (notificationIds: number[]) => {
      const currentNotifications = this.notificationsSubject.value;
      const updatedNotifications = currentNotifications.map(notification =>
        notificationIds.includes(notification.id) ? { ...notification, isRead: true } : notification
      );
      this.notificationsSubject.next(updatedNotifications);
    });

    this.hubConnection.on('AllNotificationsMarkedAsRead', () => {
      const currentNotifications = this.notificationsSubject.value;
      const updatedNotifications = currentNotifications.map(notification => ({ ...notification, isRead: true }));
      this.notificationsSubject.next(updatedNotifications);

      this.unreadCountSubject.next(updatedNotifications.filter(n => !n.isRead).length);
    });

    this.hubConnection.on('UnreadNotificationsCount', (count: number) => {
      this.unreadCountSubject.next(count);
    });

    this.hubConnection.on('ReceiveNotifications', (notifications: Notification[]) => {
      this.notificationsSubject.next(notifications);
    });

    this.hubConnection.start().catch(err => console.error('Error while starting SignalR connection:', err));
  }

  getNotifications(page: number = 1, pageSize: number = 10): Observable<{ items: Notification[], totalCount: number }> {
    return this.http.get<{ items: Notification[], totalCount: number }>(`${this.apiUrl}`, {
      params: { page: page.toString(), pageSize: pageSize.toString() }
    });
  }

  getUnreadCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/unread-count`);
  }

  markAsRead(notificationIds: number[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/mark-as-read`, notificationIds);
  }

  markAllAsRead(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/mark-all-as-read`, {});
  }

  sendNotification(userId: number, title: string, message: string, type: NotificationType, data?: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/send`, {
      userId,
      title,
      message,
      type,
      data
    });
  }

  sendNotificationToUsers(userIds: number[], title: string, message: string, type: NotificationType, data?: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/notification/send-to-users`, {
      userIds,
      title,
      message,
      type,
      data
    });
  }
}
