using Doerly.DataTransferObjects;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Infrastructure.Api;
using Doerly.Domain.Models;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[Authorize]
[ApiController]
[Area("profile")]
[Route("api/[area]")]
public class ProfileController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{userId:int}")]
    [ProducesResponseType<OperationResult<ProfileDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<ProfileDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(int userId)
    {
        var result = await ResolveHandler<GetProfileHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType<OperationResult<IEnumerable<ProfileDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfiles()
    {
        var result = await ResolveHandler<GetAllShortProfilesHandler>().HandleAsync();
        return Ok(result);
    }

    [HttpPost("_search")]
    [ProducesResponseType<OperationResult<PageDto<ProfileDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchProfiles(ProfileQueryDto queryDto)
    {
        var result = await ResolveHandler<SearchProfilesHandler>().HandleAsync(queryDto);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<OperationResult<int>>(StatusCodes.Status201Created)]
    [ProducesResponseType<OperationResult<ProfileDto>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfile(ProfileSaveDto dto)
    {
        var result = await ResolveHandler<CreateProfileHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return Conflict(result);

        return Created();
    }

    [HttpPut]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(ProfileSaveDto dto)
    {
        var result = await ResolveHandler<UpdateProfileHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{userId:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfile(int userId)
    {
        var result = await ResolveHandler<DeleteProfileHandler>().HandleAsync(userId);
        return Ok(result);
    }

    [HttpPost("{userId:int}/image")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadProfileImage([FromRoute] int userId, [FromForm] IFormFile imageFile)
    {
        var result = await ResolveHandler<UploadProfileImageHandler>().HandleAsync(userId, GetFormFileBytes(imageFile));
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{userId:int}/image")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfileImage([FromRoute] int userId)
    {
        var result = await ResolveHandler<DeleteProfileImageHandler>().HandleAsync(userId);
        return Ok(result);
    }

    [HttpPost("{userId:int}/cv")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadProfileCv([FromRoute] int userId, [FromForm] IFormFile cvFile)
    {
        var result = await ResolveHandler<UploadProfileCvHandler>().HandleAsync(userId, GetFormFileBytes(cvFile));
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{userId:int}/cv")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfileCv([FromRoute] int userId)
    {
        var result = await ResolveHandler<DeleteProfileCvHandler>().HandleAsync(userId);
        return Ok(result);
    }

    [HttpPost("{userId:int}/is-enabled")]
    public async Task<IActionResult> SetIsEnabled([FromRoute] int userId, [FromBody] EnableUserDto dto)
    {
        var result = await ResolveHandler<SetProfileIsEnabledHandler>().HandleAsync(userId, dto);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("payments-history")]
    public async Task<IActionResult> GetUserPayments([FromQuery] CursorPaginationRequest request)
    {
        var result = await ResolveHandler<SelectUserPaymentsHistory>().HandleAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId:int}/feedbacks")]
    public async Task<IActionResult> GetUserFeedbacks([FromRoute] int userId, [FromQuery] CursorPaginationRequest request)
    {
        var result = await ResolveHandler<SelectUserFeedbacksHandler>().HandleAsync(userId, request);
        return Ok(result);
    }
}
