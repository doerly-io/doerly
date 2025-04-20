using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[ApiController]
[Area("profile")]
[Route("api/[area]")]
public class ProfileController : BaseApiController
{
    [HttpGet("{userId:int}")]
    [ProducesResponseType<HandlerResult<ProfileDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<ProfileDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(int userId)
    {
        var result = await ResolveHandler<GetProfileHandler>().HandleAsync(userId);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType<HandlerResult<int>>(StatusCodes.Status201Created)]
    [ProducesResponseType<HandlerResult<ProfileDto>>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfile(ProfileSaveDto dto)
    {
        var result = await ResolveHandler<CreateProfileHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return Conflict(result);

        return Created();
    }

    [HttpPut]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(ProfileSaveDto dto)
    {
        var result = await ResolveHandler<UpdateProfileHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{userId:int}")]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfile(int userId)
    {
        var result = await ResolveHandler<DeleteProfileHandler>().HandleAsync(userId);
        return Ok(result);
    }

    [HttpPost("{userId:int}/image")]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadProfileImage([FromRoute] int userId, [FromForm] IFormFile imageFile)
    {
        using var stream = new MemoryStream();
        await imageFile.CopyToAsync(stream);
        var fileBytes = stream.ToArray();

        var result = await ResolveHandler<UploadProfileImageHandler>().HandleAsync(userId, fileBytes);
        return Ok(result);
    }
}
