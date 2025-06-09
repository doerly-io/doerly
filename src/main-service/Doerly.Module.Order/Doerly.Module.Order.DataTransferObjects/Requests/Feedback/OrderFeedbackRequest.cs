using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.DataTransferObjects.Requests;

public class OrderFeedbackRequest
{
    [Range(1, 5)]
    public required int Rating { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }
}
