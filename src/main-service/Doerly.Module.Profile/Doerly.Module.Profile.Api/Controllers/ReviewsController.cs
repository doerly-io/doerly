using Doerly.DataTransferObjects;
using Doerly.Infrastructure.Api;
using Doerly.Localization;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[Authorize]
[ApiController]
[Area("Profile")]
[Route("api/[area]/{profileId:int}/[controller]")]
public class ReviewsController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType<CursorPaginationResponse<ReviewResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews([FromRoute] int profileId, [FromQuery] CursorPaginationRequest cursorPaginationRequest)
    {
        var result = await ResolveHandler<GetProfileReviewsHandler>().HandleAsync(profileId, cursorPaginationRequest);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddReview([FromRoute] int profileId, [FromBody] AddReviewDto addReviewDto)
    {
        var userId = RequestContext.UserId;
        if (userId == null)
            return BadRequest(Resources.Get("FailedToAddReview"));

        await ResolveHandler<AddProfileReviewHandler>().HandleAsync(profileId, userId.Value, addReviewDto);
        return Created();
    }
    
    [HttpPut("{reviewId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview([FromRoute] int reviewId, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var userId = RequestContext.UserId;
        if (userId == null)
            return BadRequest(Resources.Get("FailedToUpdateReview"));

        var result = await ResolveHandler<UpdateProfileReviewHandler>().HandleAsync(userId.Value, reviewId, updateReviewDto);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
    
    [HttpDelete("{reviewId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteReview([FromRoute] int reviewId)
    {
        var userId = RequestContext.UserId;
        if (userId == null)
            return BadRequest(Resources.Get("FailedToDeleteReview"));

        var result = await ResolveHandler<DeleteProfileReviewHandler>().HandleAsync(userId.Value, reviewId);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
