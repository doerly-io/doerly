using Doerly.Domain;
using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Notification.Domain.Handlers;

public class GetUnreadNotificationsCountHandler(NotificationDbContext dbContext) : BaseNotificationHandler(dbContext)
{
    public async Task<OperationResult<int>> HandleAsync(int userId)
    {
        var count = await DbContext.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);

        return OperationResult.Success(count);
    }
} 