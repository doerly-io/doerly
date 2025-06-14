using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataAccess;
using Doerly.Module.Notification.DataAccess.Entities;
using Doerly.Module.Notification.DataTransferObjects.Responses;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Notification.Domain.Handlers;

public class GetNotificationsHandler : BaseNotificationHandler
{
    public GetNotificationsHandler(NotificationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OperationResult<CursorPaginationResponse<NotificationDto>>> HandleAsync(int userId, CursorPaginationRequest cursorRequest)
    {
        var query = DbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId);

        var cursor = Cursor.Decode(cursorRequest.Cursor);
        if (cursor.LastId != null)
            query = query.Where(p => p.Id > cursor.LastId);

        var notifications = await query
            .OrderByDescending(x => x.LastModifiedDate)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                Timestamp = n.Timestamp,
                Data = n.Data
            })
            .Take(cursorRequest.PageSize + 1)
            .ToListAsync();

        var hasMode = notifications.Count > cursorRequest.PageSize;
        string? nextCursor = null;
        if (hasMode)
        {
            var lastId = notifications.Last().Id;
            nextCursor = Cursor.Encode(lastId);
            notifications.RemoveAt(cursorRequest.PageSize);
        }

        var response = new CursorPaginationResponse<NotificationDto>
        {
            Items = notifications,
            Cursor = nextCursor
        };

        return OperationResult.Success(response);
    }
}