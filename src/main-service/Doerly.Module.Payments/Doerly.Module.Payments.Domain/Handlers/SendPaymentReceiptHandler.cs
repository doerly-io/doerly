using Doerly.Domain.Factories;
using Doerly.Helpers;
using Doerly.Localization;
using Doerly.Module.Payments.DataTransferObjects.Messages;
using Doerly.Module.Payments.DataAccess;
using Doerly.Notification.EmailSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.Handlers;

public class SendPaymentReceiptHandler : BasePaymentHandler
{
    private const string PaymentDateTemplate = "{{PaymentDate}}";
    private const string PaymentMethodTemplate = "{{PaymentMethod}}";
    private const string PaymentCardNumberTemplate = "{{PaymentCardNumber}}";
    private const string PaymentDescriptionTemplate = "{{PaymentDescription}}";
    private const string PaymentTotalPaidTemplate = "{{TotalPaid}}";

    private readonly IHandlerFactory _handlerFactory;
    private readonly ILogger<SendPaymentReceiptHandler> _logger;

    public SendPaymentReceiptHandler(
        PaymentDbContext dbContext,
        IHandlerFactory handlerFactory,
        ILogger<SendPaymentReceiptHandler> logger)
        : base(dbContext)
    {
        _handlerFactory = handlerFactory;
        _logger = logger;
    }

    public async Task HandleAsync(BillStatusChangedMessage model)
    {
        var payment = await DbContext.Payments
            .Where(x => x.Guid == model.PaymentGuid && x.Status == model.Status)
            .Select(x => new
                { x.Amount, x.Bill.PayerEmail, x.DateCreated, x.CardNumber, x.PaymentMethod, x.Description })
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            _logger.LogWarning("Payment not found for PaymentGuid: {PaymentGuid}", model.PaymentGuid);
            return;
        }

        var emailBody = EmailTemplate.Get(EmailTemplate.PaymentReceipt);

        emailBody = emailBody
            .Replace(PaymentDateTemplate, payment.DateCreated.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"))
            .Replace(PaymentMethodTemplate, payment.PaymentMethod.ToDescription())
            .Replace(PaymentCardNumberTemplate, payment.CardNumber ?? string.Empty)
            .Replace(PaymentDescriptionTemplate, payment.Description)
            .Replace(PaymentTotalPaidTemplate, payment.Amount.ToString("C"));

        var sendEmailHandler = _handlerFactory.Get<SendEmailHandler>();
        var result = await sendEmailHandler.HandleAsync(payment.PayerEmail, Resources.Get("PaymentReceipt"), emailBody);

        if (!result.IsSuccess)
            _logger.LogError("Failed to send payment receipt email for PaymentGuid: {PaymentGuid}. Error: {Error}",
                model.PaymentGuid,
                result.ErrorMessage);
        else
            _logger.LogInformation("Payment receipt email sent successfully for PaymentGuid: {PaymentGuid}",
                model.PaymentGuid);
    }
}
