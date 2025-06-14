using Doerly.DataAccess.Models;
using Doerly.Module.Notification.Enums;

namespace Doerly.Module.Notification.DataAccess.Entities;

public class NotificationEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public string? Data { get; set; } // JSON data for additional information
    public DateTime Timestamp { get; set; }
} 