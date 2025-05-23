using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class AddReviewDto
{
    [Required(ErrorMessage = "RatingRequired")]
    [Range(1,5, ErrorMessage = "ExceededRatingRange")]
    public required int Rating { get; set; }

    [Required(ErrorMessage = "CommentRequired")]
    [MaxLength(2000, ErrorMessage = "ExceededReviewCommentLength")]
    public required string Comment { get; set; }
    
    [Required(ErrorMessage = "ReviewerIdRequired")]
    public required int ReviewerId { get; set; }

    [Required(ErrorMessage = "RevieweeIdRequired")]
    public required int RevieweeId { get; set; }
}
