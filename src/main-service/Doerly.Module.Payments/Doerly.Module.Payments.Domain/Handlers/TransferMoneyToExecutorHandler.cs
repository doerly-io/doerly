using Doerly.Module.Payments.DataAccess;
using Doerly.Module.Payments.DataTransferObjects.Messages;
using Doerly.Module.Payments.Enums;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.Handlers;

public class TransferMoneyToExecutorHandler : BasePaymentHandler
{
    private readonly ILogger<TransferMoneyToExecutorHandler> _logger;

    public TransferMoneyToExecutorHandler(
        PaymentDbContext dbContext,
        ILogger<TransferMoneyToExecutorHandler> logger)
        : base(dbContext)
    {
        _logger = logger;
    }

    public async Task HandleAsync(BillStatusChangedMessage billStatusChangedMessage)
    {
        if (billStatusChangedMessage.Status != EPaymentStatus.Completed)
        {
            _logger.LogError("Payment error from transfer money to executor handler. PaymentGuid: {PaymentGuid}",
                billStatusChangedMessage.PaymentGuid);
            return;
        }
        
        //process taxes here, implement after consultation with tax department
        
        //transfer money to executor
        
        
    }
}