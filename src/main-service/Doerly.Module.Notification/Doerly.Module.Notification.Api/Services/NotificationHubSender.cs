using Doerly.Module.Notification.Api.Hubs;
using Doerly.Module.Notification.DataTransferObjects.Responses;
using Doerly.Module.Notification.Domain.Services;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Notification.Api.Services;

public class NotificationHubSender : INotificationSender
{
    private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

    public NotificationHubSender(IHubContext<NotificationHub, INotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(NotificationDto notification)
    {
        await _hubContext.Clients.User(notification.UserId.ToString()).ReceiveNotification(notification);
    }

    public async Task SendNotificationsAsync(IEnumerable<NotificationDto> notifications)
    {
        var groupedNotifications = notifications.GroupBy(n => n.UserId);
        foreach (var group in groupedNotifications)
        {
            await _hubContext.Clients.User(group.Key.ToString()).ReceiveNotifications(group);
        }
    }

    public async Task UpdateUnreadCountAsync(int userId, int count)
    {
        await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(count);
    }

    public async Task MarkNotificationsAsReadAsync(int userId, int[] notificationIds)
    {
        await _hubContext.Clients.User(userId.ToString()).NotificationsMarkedAsRead(notificationIds);
    }

    public async Task MarkAllNotificationsAsReadAsync(int userId)
    {
        await _hubContext.Clients.User(userId.ToString()).AllNotificationsMarkedAsRead();
    }
} 