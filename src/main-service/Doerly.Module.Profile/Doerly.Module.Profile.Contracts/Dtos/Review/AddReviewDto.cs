using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class AddReviewDto : UpdateReviewDto
{
    [Required(ErrorMessage = "ReviewerIdRequired")]
    public required int ReviewerUserId { get; set; }
}
