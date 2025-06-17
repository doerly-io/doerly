using Doerly.DataTransferObjects;
using Doerly.Domain.Exceptions;
using Doerly.Domain.Factories;
using Doerly.Module.Notification.Api.Hubs;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Notification.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

    public NotificationController(
        IHandlerFactory handlerFactory,
        IHubContext<NotificationHub, INotificationHub> hubContext)
    {
        _handlerFactory = handlerFactory;
        _hubContext = hubContext;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification(int userId, string title, NotificationType type, DateTime timestamp, string? data = null)
    {
        var result = await _handlerFactory.Get<SendNotificationHandler>().HandleAsync(userId, title, type, timestamp, data);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await _hubContext.Clients.User(userId.ToString()).ReceiveNotification(result.Value);

        // Update unread count
        var unreadCount = await _handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
        await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(unreadCount.Value);

        return Ok();
    }

    [HttpPost("send-to-users")]
    public async Task<IActionResult> SendNotificationToUsers(IEnumerable<int> userIds, string title, NotificationType type, DateTime timestamp, string? data = null)
    {
        foreach (var userId in userIds)
        {
            var result = await _handlerFactory.Get<SendNotificationHandler>().HandleAsync(userId, title, type, timestamp, data);
            if (!result.IsSuccess)
            {
                throw new DoerlyException(result.ErrorMessage);
            }

            await _hubContext.Clients.User(userId.ToString()).ReceiveNotification(result.Value);

            // Update unread count
            var unreadCount = await _handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
            await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(unreadCount.Value);
        }

        return Ok();
    }

    [HttpPost("mark-as-read")]
    public async Task<IActionResult> MarkNotificationsAsRead(int[] notificationIds)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var result = await _handlerFactory.Get<MarkNotificationsAsReadHandler>().HandleAsync(userId, notificationIds);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await _hubContext.Clients.User(userId.ToString()).NotificationsMarkedAsRead(notificationIds);

        // Update unread count
        var unreadCount = await _handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
        await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(unreadCount.Value);

        return Ok();
    }

    [HttpPost("mark-all-as-read")]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var result = await _handlerFactory.Get<MarkAllNotificationsAsReadHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await _hubContext.Clients.User(userId.ToString()).AllNotificationsMarkedAsRead();

        // Update unread count
        var unreadCount = await _handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
        await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(unreadCount.Value);

        return Ok();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadNotificationsCount()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var result = await _handlerFactory.Get<GetUnreadNotificationsCountHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await _hubContext.Clients.User(userId.ToString()).UnreadNotificationsCount(result.Value);
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] CursorPaginationRequest request)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var result = await _handlerFactory.Get<GetNotificationsHandler>().HandleAsync(userId, request);
        if (!result.IsSuccess)
        {
            throw new DoerlyException(result.ErrorMessage);
        }

        await _hubContext.Clients.User(userId.ToString()).ReceiveNotifications(result.Value.Items);
        return Ok(result.Value);
    }
} 