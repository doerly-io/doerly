using Doerly.Infrastructure.Api;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers.Review;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

public class ReviewsController : BaseApiController
{
    public async Task<IActionResult> AddReview([FromBody] AddReviewDto addReviewDto)
    {
        await ResolveHandler<AddProfileReviewHandler>().HandleAsync(addReviewDto);

        return Created();
    }
}
