using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Authorization.Api.Constants;
using Doerly.Module.Authorization.Domain.Dtos;
using Doerly.Module.Authorization.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Authorization.Api.Controllers;

[ApiController]
[Area("authorization")]
[Route("api/[area]/auth")]
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

        return Ok(result.Value.resultDto);
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
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();
        
        var accessToken = Request.Headers[AuthConstants.AuthorizationHeaderName].ToString();
        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        var result = await ResolveHandler<RefreshTokenHandler>().HandleAsync(refreshToken, accessToken);
        if (!result.IsSuccess)
            return Unauthorized(result);

        SetHttpCookie(AuthConstants.RefreshTokenCookieName, result.Value.refreshToken);

        return Ok(result.Value.resultDto);
    }
    
}
