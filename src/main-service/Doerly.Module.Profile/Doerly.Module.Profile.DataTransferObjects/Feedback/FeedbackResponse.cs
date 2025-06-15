using Doerly.Module.Profile.DataTransferObjects.Profile;

namespace Doerly.Module.Profile.DataTransferObjects.Feedback;

public class FeedbackResponse
{
    public int FeedbackId { get; set; }
    
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public ProfileInfo UserProfile { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}