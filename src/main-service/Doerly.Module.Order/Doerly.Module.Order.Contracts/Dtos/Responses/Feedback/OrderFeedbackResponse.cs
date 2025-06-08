namespace Doerly.Module.Order.Contracts.Dtos.Responses;

public class OrderFeedbackResponse
{
    public int FeedbackId { get; set; }
    public int Rating { get; set; }

    public string? Comment { get; set; }

    // public int ReviewerUserId { get; set; }
    //
    // public string ReviewerFullName { get; set; }
    //
    // public string AvatarUrl { get; set; }

    public ProfileInfo UserProfile { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
