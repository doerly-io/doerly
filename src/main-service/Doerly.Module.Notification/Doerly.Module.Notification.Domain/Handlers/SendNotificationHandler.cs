using Doerly.Domain.Models;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.DataAccess.Entities;
using Doerly.Module.Notification.DataTransferObjects.Responses;
using Doerly.Module.Notification.Domain.Services;
using Doerly.Module.Notification.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Notification.Domain.Handlers;

public class SendNotificationHandler(
    NotificationDbContext dbContext,
    INotificationSender notificationSender)
    : BaseNotificationHandler(dbContext)
{
    public async Task<OperationResult<NotificationDto>> HandleAsync(int userId, string message, NotificationType type, string? data = null)
    {
        var notification = new NotificationEntity
        {
            UserId = userId,
            Message = message,
            Type = type,
            IsRead = false,
            Data = data
        };

        DbContext.Notifications.Add(notification);
        await DbContext.SaveChangesAsync();

        var response = new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            Type = notification.Type,
            IsRead = notification.IsRead,
            CreatedAt = notification.DateCreated,
            Data = notification.Data
        };

        // Send notification through SignalR
        await notificationSender.SendNotificationAsync(response);

        // Update unread count
        var unreadCount = await DbContext.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        await notificationSender.UpdateUnreadCountAsync(userId, unreadCount);

        return OperationResult.Success(response);
    }
}