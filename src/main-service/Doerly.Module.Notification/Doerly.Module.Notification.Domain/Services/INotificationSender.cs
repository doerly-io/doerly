using Doerly.Module.Notification.DataTransferObjects.Responses;

namespace Doerly.Module.Notification.Domain.Services;

public interface INotificationSender
{
    Task SendNotificationAsync(NotificationDto notification);
    Task SendNotificationsAsync(IEnumerable<NotificationDto> notifications);
    Task UpdateUnreadCountAsync(int userId, int count);
    Task MarkNotificationsAsReadAsync(int userId, int[] notificationIds);
    Task MarkAllNotificationsAsReadAsync(int userId);
} 