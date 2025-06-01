using Doerly.Domain.Models;
using Doerly.Messaging;
using Doerly.Module.Payments.Client.LiqPay.Helpers;
using Doerly.Module.Payments.Contracts.Messages;
using Doerly.Module.Payments.DataAccess;
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
    
    public async Task<HandlerResult> Handle(PaymentStatusChangedMessage message)
    {
        var bill = await DbContext.Bills
            .Include(i => i.Payments.Where(x => x.Status == EPaymentStatus.Pending))
            .FirstOrDefaultAsync(i => i.Id == message.BillId);
        if (bill == null)
        {
            _logger.LogWarning("Bill not found for billId: {BillId}", message.BillId);
            return HandlerResult.Failure("Bill not found");
        }
        
        var payment = bill.Payments.FirstOrDefault();
        if (payment == null)
        {
            _logger.LogWarning("Payment not found for billId: {BillId}", message.BillId);
            return HandlerResult.Failure("Payment not found");
        }

        payment.Status = message.Status;
        bill.AmountPaid += payment.Amount;
        await DbContext.SaveChangesAsync();

        await _messagePublisher.Publish(message);
        
        return HandlerResult.Success();
    }
}

