using System.Security.Claims;
using Doerly.Domain;

namespace Doerly.Host.Middlewares;

public class RequestContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IDoerlyRequestContext requestContext)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            requestContext.UserId = userId;
        
        var userRolesClaim = context.User.FindFirst(ClaimTypes.Role);
        if (userRolesClaim != null && !string.IsNullOrEmpty(userRolesClaim.Value))
        {
            var roles = userRolesClaim.Value.Split(',');
            requestContext.UserRoles = roles.Length > 0 ? roles : [];
        }
        
        var userEmailClaim = context.User.FindFirst(ClaimTypes.Email);
        if (userEmailClaim != null)
            requestContext.UserEmail = userEmailClaim.Value;

        await next(context);
    }
}
