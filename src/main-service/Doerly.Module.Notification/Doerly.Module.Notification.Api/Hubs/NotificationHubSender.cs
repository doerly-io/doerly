using Doerly.Module.Notification.DataTransferObjects.Responses;
using Doerly.Module.Notification.Domain.Services;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Notification.Api.Hubs;

public class NotificationHubSender(IHubContext<NotificationHub, INotificationHub> hubContext) : INotificationSender
{
    public async Task SendNotificationAsync(NotificationDto notification)
    {
        await hubContext.Clients.User(notification.UserId.ToString()).ReceiveNotification(notification);
    }

    public async Task SendNotificationsAsync(IEnumerable<NotificationDto> notifications)
    {
        var groupedNotifications = notifications.GroupBy(n => n.UserId);
        foreach (var group in groupedNotifications)
        {
            await hubContext.Clients.User(group.Key.ToString()).ReceiveNotifications(group);
        }
    }

    public async Task UpdateUnreadCountAsync(int userId, int count)
    {
        await hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(count);
    }

    public async Task MarkNotificationsAsReadAsync(int userId, int[] notificationIds)
    {
        await hubContext.Clients.User(userId.ToString()).NotificationsMarkedAsRead(notificationIds);
    }

    public async Task MarkAllNotificationsAsReadAsync(int userId)
    {
        await hubContext.Clients.User(userId.ToString()).AllNotificationsMarkedAsRead();
    }
} 