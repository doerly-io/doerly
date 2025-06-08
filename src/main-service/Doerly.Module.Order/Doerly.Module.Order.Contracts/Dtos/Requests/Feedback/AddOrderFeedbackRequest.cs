using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.Contracts.Dtos.Requests;

public class AddOrderFeedbackRequest
{
    [Range(1, 5)]
    public required int Rating { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }
}
