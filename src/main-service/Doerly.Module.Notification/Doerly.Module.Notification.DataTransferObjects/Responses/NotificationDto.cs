using Doerly.Module.Notification.Enums;

namespace Doerly.Module.Notification.DataTransferObjects.Responses;

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Data { get; set; }
}