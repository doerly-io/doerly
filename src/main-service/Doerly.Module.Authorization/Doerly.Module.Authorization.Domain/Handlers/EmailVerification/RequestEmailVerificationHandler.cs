using System.Web;
using Doerly.Common;
using Doerly.Domain.Exceptions;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Authorization.DataAccess.Entities;
using Doerly.Module.Authorization.Contracts.Messages;
using Doerly.Module.Authorization.Enums;
using Doerly.Notification.EmailSender;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Authorization.Domain.Handlers;

public class RequestEmailVerificationHandler : BaseAuthHandler
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly IOptions<FrontendSettings> _frontendOptions;

    public RequestEmailVerificationHandler(
        AuthorizationDbContext dbContext,
        IOptions<AuthSettings> authOptions,
        IHandlerFactory handlerFactory,
        IOptions<FrontendSettings> frontendOptions
    )
        : base(dbContext, authOptions)
    {
        _handlerFactory = handlerFactory;
        _frontendOptions = frontendOptions;
    }

    public async Task<HandlerResult> HandleAsync(UserRegisteredMessage message)
    {
        var emailBody = EmailTemplate.Get(EmailTemplate.EmailVerification);

        var token = GetResetToken();
        var resetToken = new TokenEntity
        {
            Guid = Guid.CreateVersion7(),
            UserId = message.UserId,
            Value = token.hashedToken,
            DateCreated = DateTime.UtcNow,
            TokenKind = ETokenKind.EmailVerification,
        };
        DbContext.Tokens.Add(resetToken);
        await DbContext.SaveChangesAsync();

        var encodedToken = HttpUtility.UrlEncode(token.originalToken);
        var url = $"{_frontendOptions.Value.FrontendUrl}/auth/email-verification?token={encodedToken}&email={message.Email}";
        emailBody = emailBody.Replace("{{welcomeVerificationText}}", Resources.Get("EmailVerificationText"));
        emailBody = emailBody.Replace("{{verificationLinkText}}", $"{url}");
        emailBody = emailBody.Replace("{{verificationLink}}", $"{url}");
        emailBody = emailBody.Replace("{{bestRegardsText}}", Resources.Get("ResetPasswordEmailBestRegardsText"));

        var result = await _handlerFactory.Get<SendEmailHandler>().HandleAsync(message.Email, "Email Verification", emailBody);
        if (!result.IsSuccess)
            throw new DoerlyException(result.ErrorMessage);


        return HandlerResult.Success();
    }
}
