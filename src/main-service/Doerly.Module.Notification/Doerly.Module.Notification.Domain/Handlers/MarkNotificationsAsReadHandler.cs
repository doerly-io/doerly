using Doerly.Domain;
using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Notification.Domain.Handlers;

public class MarkNotificationsAsReadHandler(NotificationDbContext dbContext) : BaseNotificationHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(int userId, int[] notificationIds)
    {
        var notifications = await DbContext.Notifications
            .Where(n => n.UserId == userId && notificationIds.Contains(n.Id))
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await DbContext.SaveChangesAsync();
        return OperationResult.Success();
    }
} 