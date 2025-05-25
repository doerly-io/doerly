using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class UpdateReviewDto
{
    [Required(ErrorMessage = "RatingRequired")]
    [Range(1,5, ErrorMessage = "ExceededRatingRange")]
    public required int Rating { get; set; }

    [Required(ErrorMessage = "CommentRequired")]
    [MinLength(5, ErrorMessage = "CommentMinLength")]
    [MaxLength(2000, ErrorMessage = "ExceededReviewCommentLength")]
    public required string Comment { get; set; }
    
}
