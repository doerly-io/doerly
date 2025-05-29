using System.Diagnostics;
using System.Security.Claims;
using Doerly.Common.Settings.Constants;
using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Infrastructure.Api;

public class BaseApiController : ControllerBase
{
    protected IDoerlyRequestContext RequestContext => HttpContext.RequestServices.GetRequiredService<IDoerlyRequestContext>();
    
    [DebuggerStepThrough]
    protected THandler ResolveHandler<THandler>() where THandler : IHandler
    {
        var handlerFactory = HttpContext.RequestServices.GetRequiredService<IHandlerFactory>();
        return handlerFactory.Get<THandler>();
    }

    [NonAction]
    protected void SetHttpCookie(string key, string value)
    {
        Response.Cookies.Append(key, value, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });
    }

    [NonAction]
    protected string GetBearerToken()
    {
        var authorizationHeader = Request.Headers[AuthConstants.AuthorizationHeaderName].ToString();
        return authorizationHeader.Replace("Bearer ", "");
    }
    
    [NonAction]
    protected byte[] GetFormFileBytes(IFormFile file)
    {
        using var stream = new MemoryStream();
        file.CopyTo(stream);
        return stream.ToArray();
    }
    
    [NonAction]
    protected int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException();
        }

        return userId;
    }
}
