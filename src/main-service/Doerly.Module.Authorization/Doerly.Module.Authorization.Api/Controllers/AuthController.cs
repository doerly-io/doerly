using System.ComponentModel.DataAnnotations;
using System.Web;
using Doerly.Infrastructure.Api;
using Doerly.Domain.Models;
using Doerly.Messaging;
using Doerly.Module.Authorization.Api.Constants;
using Doerly.Module.Authorization.Contracts.Dtos;
using Doerly.Module.Authorization.Contracts.Messages;
using Doerly.Module.Authorization.Domain.Handlers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Authorization.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMessagePublisher messagePublisher) : BaseApiController
{
    [HttpPost("login")]
    [ProducesResponseType<HandlerResult<LoginResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<LoginResponseDto>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto requestDto)
    {
        var result = await ResolveHandler<LoginHandler>().HandleAsync(requestDto);
        if (!result.IsSuccess)
            return Unauthorized(result);

        SetHttpCookie(AuthConstants.RefreshTokenCookieName, result.Value.refreshToken);

        return Ok(HandlerResult.Success(result.Value.resultDto));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequestDto requestDto)
    {
        var result = await ResolveHandler<RegistrationHandler>().HandleAsync(requestDto);
        if (result.IsSuccess)
            return Created();

        return Conflict(result);
    }

    [HttpGet("refresh")]
    [ProducesResponseType<HandlerResult<LoginResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<LoginResponseDto>>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];
        var accessToken = GetBearerToken();

        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        var decodedToken = HttpUtility.UrlDecode(refreshToken);
        var result = await ResolveHandler<RefreshTokenHandler>().HandleAsync(decodedToken, accessToken);
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

        await ResolveHandler<LogoutHandler>().HandleAsync(refreshToken);
        Response.Cookies.Delete(AuthConstants.RefreshTokenCookieName);

        return Ok();
    }


    //ToDo: enable ratelimiting
    [HttpGet("password-reset/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RequestPasswordReset([EmailAddress(ErrorMessage = "InvalidEmailFormatInput")] string email)
    {
        await messagePublisher.Publish(new PasswordResetRequestMessage(email));
        return Ok();
    }

    //ToDo: enable ratelimiting
    [HttpPost("password-reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PasswordReset(ResetPasswordRequestDto requestDto)
    {
        var result = await ResolveHandler<ResetPasswordHandler>().HandleAsync(requestDto);
        if (result.IsSuccess)
            return Ok();

        return NotFound(result);
    }

    //ToDo: enable ratelimiting
    [HttpGet("email-verification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EmailVerification([FromQuery] string token, [FromQuery] string email)
    {
        var result = await ResolveHandler<EmailVerificationHandler>().HandleAsync(token, email);
        if (result.IsSuccess)
            return Ok();

        return NotFound(result);
    }
}
