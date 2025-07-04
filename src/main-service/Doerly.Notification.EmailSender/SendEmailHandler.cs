using System.Net;
using Doerly.Common.Settings;
using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Doerly.Notification.EmailSender;

public class SendEmailHandler : BaseHandler
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IOptions<SendGridSettings> _options;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(ISendGridClient sendGridClient,
        IOptions<SendGridSettings> options,
        ILogger<SendEmailHandler> logger
    )
    {
        _sendGridClient = sendGridClient;
        _options = options;
        _logger = logger;
    }

    public async Task<OperationResult> HandleAsync(string email, string subject, string body)
    {
        var emailFromAddress = new EmailAddress(_options.Value.SenderEmail);
        var emailToAddress = new EmailAddress(email);

        var message = MailHelper.CreateSingleEmail(emailFromAddress, emailToAddress, subject, body, body);

        _logger.LogInformation("Sending email to {email}.", email);

        var result = await _sendGridClient.SendEmailAsync(message);
        
        if (result.StatusCode != HttpStatusCode.Accepted)
            return OperationResult.Failure($"Failed to send email. Status code: {result.StatusCode}.");
        
        return OperationResult.Success();
    }
}
