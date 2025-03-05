using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.Api.Constants;
using Doerly.Module.Authorization.Domain.Dtos;
using Doerly.Module.Authorization.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Authorization.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    [ProducesResponseType<HandlerResult<LoginResultDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<LoginResultDto>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await ResolveHandler<LoginHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return Unauthorized(result);

        SetHttpCookie(AuthConstants.RefreshTokenCookieName, result.Value.refreshToken);

        return Ok(HandlerResult.Success(result.Value.resultDto));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await ResolveHandler<RegisterHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Created();

        return Conflict(result);
    }

    [HttpGet("refresh")]
    [ProducesResponseType<HandlerResult<LoginResultDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<LoginResultDto>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];
        var accessToken = GetBearerToken();

        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        var result = await ResolveHandler<RefreshTokenHandler>().HandleAsync(refreshToken, accessToken);
        if (!result.IsSuccess)
            return Unauthorized(result);

        SetHttpCookie(AuthConstants.RefreshTokenCookieName, result.Value.refreshToken);

        return Ok(HandlerResult.Success(result.Value.resultDto));
    }

    [Authorize]
    [HttpGet("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];
        if (string.IsNullOrEmpty(refreshToken))
            return Ok();
    
        var result = await ResolveHandler<LogoutHandler>().HandleAsync(refreshToken);
        Response.Cookies.Delete(AuthConstants.RefreshTokenCookieName);
    
        return Ok();
    }


    //ToDo: enable ratelimiting
    [HttpGet("password-reset/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string email)
    {
        var result = await ResolveHandler<RequestPasswordResetHandler>().HandeAsync(email);
        return Accepted(result);
    }

    //ToDo: enable ratelimiting
    [HttpPost("password-reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(ResetPasswordDto dto)
    {
        var result = await ResolveHandler<ResetPasswordHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok();

        return NotFound(result);
    }
}
