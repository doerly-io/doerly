using System.Security.Cryptography;
using Doerly.Common;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Models;
using Doerly.Notification.EmailSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RequestPasswordResetHandler : BaseAuthHandler
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly IOptions<FrontendSettings> _frontedOptions;

    public RequestPasswordResetHandler(
        AuthorizationDbContext dbContext,
        IHandlerFactory handlerFactory,
        IOptions<AuthSettings> authOptions,
        IOptions<FrontendSettings> frontedOptions) : base(dbContext, authOptions)
    {
        _handlerFactory = handlerFactory;
        _frontedOptions = frontedOptions;
    }

    public async Task<HandlerResult> HandeAsync(string email)
    {
        var userId = await DbContext.Users
            .Where(x => x.Email == email)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync();

        //if the user does not exist, we still need to show generic message to the user for security reasons
        if (userId == null)
            return HandlerResult.Success();

        var token = GetResetToken();
        var resetToken = new Token
        {
            Guid = Guid.NewGuid(),
            UserId = userId.Value,
            Value = token.hashedToken,
            DateCreated = DateTime.UtcNow
        };
        DbContext.Tokens.Add(resetToken);
        await DbContext.SaveChangesAsync();

        var emailBody = EmailTemplate.Get(EmailTemplate.ResetPassword);

        var url = $"{_frontedOptions.Value.FrontendUrl}/auth/password-reset?token={token.originalToken}";
        emailBody = emailBody.Replace("{{welcomeVerificationText}}", Resources.Get("ResetPasswordEmailVerificationText"));
        emailBody = emailBody.Replace("{{verificationLinkText}}", $"{url}");
        emailBody = emailBody.Replace("{{verificationLink}}", $"{url}");
        emailBody = emailBody.Replace("{{bestRegardsText}}", Resources.Get("ResetPasswordEmailBestRegardsText"));

        await _handlerFactory.Get<SendEmailHandler>().HandleAsync(email, "Reset Password", emailBody);

        return HandlerResult.Success();
    }
}
