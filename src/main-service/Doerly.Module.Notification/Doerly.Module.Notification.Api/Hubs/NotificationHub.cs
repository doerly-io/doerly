using System.Security.Claims;
using Doerly.DataTransferObjects;
using Doerly.Domain.Exceptions;
using Doerly.Domain.Factories;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Notification.Api.Hubs;

[Authorize]
public class NotificationHub(IHandlerFactory handlerFactory) : Hub<INotificationHub>
{
    public async Task SendNotification(int userId, string title, string message, NotificationType type, string? data = null)
    {
        var result = await handlerFactory.Get<SendNotificationHandler>().HandleAsync(userId, title, message, type, data);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }
    }

    public async Task SendNotificationToUsers(IEnumerable<int> userIds, string title, string message, NotificationType type, string? data = null)
    {
        foreach (var userId in userIds)
        {
            await SendNotification(userId, title, message, type, data);
        }
    }

    public async Task MarkNotificationsAsRead(int[] notificationIds)
    {
        var userId = GetUserId();
        var result = await handlerFactory.Get<MarkNotificationsAsReadHandler>().HandleAsync(userId, notificationIds);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await Clients.Caller.NotificationsMarkedAsRead(notificationIds);
    }

    public async Task MarkAllNotificationsAsRead()
    {
        var userId = GetUserId();
        var result = await handlerFactory.Get<MarkAllNotificationsAsReadHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await Clients.Caller.AllNotificationsMarkedAsRead();
    }

    public async Task GetUnreadNotificationsCount()
    {
        var userId = GetUserId();
        var result = await handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await Clients.Caller.UnreadNotificationsCount(result.Value);
    }

    public async Task GetNotifications(int? cursor = null, int pageSize = 20)
    {
        var userId = GetUserId();
        var result = await handlerFactory.Get<GetNotificationsHandler>().HandleAsync(userId, new CursorPaginationRequest
        {
            Cursor = cursor?.ToString(),
            PageSize = pageSize
        });
        
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await Clients.Caller.ReceiveNotifications(result.Value.Items);
    }

    public override async Task OnConnectedAsync()
    {
        if (GetUserId() > 0)
        {
            await base.OnConnectedAsync();
        }
        else
        {
            Context.Abort();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    private int GetUserId()
    {
        return int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();
    }
} 