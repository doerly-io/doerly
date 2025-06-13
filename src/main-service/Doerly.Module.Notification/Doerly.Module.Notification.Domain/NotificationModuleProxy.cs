using Doerly.DataTransferObjects;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataTransferObjects.Responses;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using Doerly.Module.Notification.Proxy;

namespace Doerly.Module.Notification.Domain;

public class NotificationModuleProxy(IHandlerFactory handlerFactory) : INotificationModuleProxy
{
    public Task<OperationResult<NotificationDto>> SendNotificationAsync(int userId, string title, string message, NotificationType type, string? data = null)
    {
        return handlerFactory.Get<SendNotificationHandler>().HandleAsync(userId, title, message, type, data);
    }

    public async Task<OperationResult> SendNotificationToUsersAsync(IEnumerable<int> userIds, string title, string message, NotificationType type, string? data = null)
    {
        foreach (var userId in userIds)
        {
            var result = await SendNotificationAsync(userId, title, message, type, data);
            if (!result.IsSuccess)
            {
                return result;
            }
        }
        return OperationResult.Success();
    }

    public Task<OperationResult> MarkNotificationsAsReadAsync(int userId, int[] notificationIds)
    {
        return handlerFactory.Get<MarkNotificationsAsReadHandler>().HandleAsync(userId, notificationIds);
    }

    public Task<OperationResult> MarkAllNotificationsAsReadAsync(int userId)
    {
        return handlerFactory.Get<MarkAllNotificationsAsReadHandler>().HandleAsync(userId);
    }

    public Task<OperationResult<int>> GetUnreadNotificationsCountAsync(int userId)
    {
        return handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
    }

    public Task<OperationResult<CursorPaginationResponse<NotificationDto>>> GetNotificationsAsync(int userId, string? cursor = null, int pageSize = 20)
    {
        return handlerFactory.Get<GetNotificationsHandler>().HandleAsync(userId, new CursorPaginationRequest(){ Cursor = cursor, PageSize = pageSize});
    }
} 