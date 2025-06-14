namespace Doerly.Module.Notification.Contracts.Responses;

public class NotificationResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Data { get; set; }
} 