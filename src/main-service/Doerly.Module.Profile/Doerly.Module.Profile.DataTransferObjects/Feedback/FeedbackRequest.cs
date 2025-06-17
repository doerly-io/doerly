using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Profile.DataTransferObjects.Feedback;

public class FeedbackRequest
{
    [Range(1, 5)]
    public required int Rating { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    public int CategoryId { get; set; }
    
    public int ExecutorId { get; set; }
}
