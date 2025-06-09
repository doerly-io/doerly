using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Order.Domain.Handlers;
using Doerly.Module.Payments.DataTransferObjects.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Order.Domain.EventConsumers;

public class PaymentStatusConsumer : BaseConsumer<BillStatusChangedMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public PaymentStatusConsumer(
        ILogger<PaymentStatusConsumer> logger,
        IDoerlyRequestContext doerlyRequestContext,
        IHandlerFactory handlerFactory) : base(logger, doerlyRequestContext)
    {
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<BillStatusChangedMessage> context)
    {
        await _handlerFactory.Get<UpdateOrderPaymentStatusHandler>().HandleAsync(context.Message.BillId,
            context.Message.Status == Payments.Enums.EPaymentStatus.Completed);
    }
}
