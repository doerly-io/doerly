using Doerly.Domain.Models;
using Doerly.Messaging;
using Doerly.Module.Payments.Contracts.Messages;
using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.Domain.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.Handlers;

public class PaymentStatusChangedHandler : BasePaymentHandler
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<PaymentStatusChangedHandler> _logger;

    public PaymentStatusChangedHandler(
        PaymentDbContext dbContext,
        IMessagePublisher messagePublisher,
        ILogger<PaymentStatusChangedHandler> logger
    ) : base(dbContext)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task<HandlerResult> Handle(PaymentStatusChangedModel model)
    {
        var payment = await DbContext.Payments
            .Include(x => x.Bill)
            .FirstOrDefaultAsync(x => x.Guid == model.PaymentGuid && x.Status == EPaymentStatus.Pending);

        if (payment == null)
        {
            _logger.LogWarning("Pending payment not found for PaymentGuid: {PaymentGuid}", model.PaymentGuid);
            return HandlerResult.Failure("Payment not found");
        }

        payment.Status = model.Status;
        payment.Bill.AmountPaid += payment.Amount;
        payment.PaymentMethod = model.PaymentMethod;
        payment.CardNumber = model.CardNumber;
        await DbContext.SaveChangesAsync();

        await PublishPaymentStatusChangedEvent(payment.BillId, payment.Guid, model.Status);

        return HandlerResult.Success();
    }

    private async Task PublishPaymentStatusChangedEvent(int billId, Guid paymentGuid, EPaymentStatus status)
    {
        var message = new BillStatusChangedMessage(billId, paymentGuid, status);
        await _messagePublisher.Publish(message);
    }
}
