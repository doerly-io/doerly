using Doerly.Domain;
using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Notification.Domain.Handlers;

public class MarkAllNotificationsAsReadHandler(NotificationDbContext dbContext) : BaseNotificationHandler(dbContext)
{
    public async Task<OperationResult> HandleAsync(int userId)
    {
        var notifications = await DbContext.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await DbContext.SaveChangesAsync();
        return OperationResult.Success();
    }
} 