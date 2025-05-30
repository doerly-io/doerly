using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Order.Domain.Handlers.Order;
using Doerly.Module.Payments.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Order.Domain.EventConsumers;
public class PaymentStatusConsumer : BaseConsumer<PaymentStatusChangedMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public PaymentStatusConsumer(ILogger<PaymentStatusConsumer> logger, IHandlerFactory handlerFactory) : base(logger)
    {
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<PaymentStatusChangedMessage> context)
    {
        await _handlerFactory.Get<UpdateOrderPaymentStatusHandler>().HandleAsync(context.Message.BillId, context.Message.Status == Payments.Enums.EPaymentStatus.Completed);
    }
}
