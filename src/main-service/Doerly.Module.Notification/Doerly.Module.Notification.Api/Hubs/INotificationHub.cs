using Doerly.Module.Notification.DataTransferObjects.Responses;

namespace Doerly.Module.Notification.Api.Hubs;

public interface INotificationHub
{
    Task ReceiveNotification(NotificationDto notification);
    Task NotificationsMarkedAsRead(int[] notificationIds);
    Task AllNotificationsMarkedAsRead();
    Task UnreadNotificationsCount(int count);
    Task ReceiveNotifications(IEnumerable<NotificationDto> notifications);
} 