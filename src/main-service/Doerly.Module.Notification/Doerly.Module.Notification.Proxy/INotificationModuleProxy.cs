using Doerly.DataTransferObjects;
using Doerly.Domain.Models;
using Doerly.Module.Notification.DataTransferObjects.Responses;
using Doerly.Module.Notification.Enums;
using Doerly.Proxy.BaseProxy;

namespace Doerly.Module.Notification.Proxy;

public interface INotificationModuleProxy : IModuleProxy
{
    Task<OperationResult<NotificationDto>> SendNotificationAsync(int userId, string message, NotificationType type, string? data = null);
    Task<OperationResult> SendNotificationToUsersAsync(IEnumerable<int> userIds, string message, NotificationType type, string? data = null);
    Task<OperationResult> MarkNotificationsAsReadAsync(int userId, int[] notificationIds);
    Task<OperationResult> MarkAllNotificationsAsReadAsync(int userId);
    Task<OperationResult<int>> GetUnreadNotificationsCountAsync(int userId);
    Task<OperationResult<CursorPaginationResponse<NotificationDto>>> GetNotificationsAsync(int userId, string? cursor = null, int pageSize = 20);
} 