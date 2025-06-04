using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Payments.Contracts.Messages;
using Doerly.Module.Payments.Domain.Handlers;
using Doerly.Module.Payments.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.EventConsumers;

public class PaymentStatusEventConsumer : BaseConsumer<BillStatusChangedMessage>
{
    private readonly IDoerlyRequestContext _requestContext;
    private readonly IHandlerFactory _handlerFactory;

    public PaymentStatusEventConsumer(
        IDoerlyRequestContext requestContext,
        IHandlerFactory handlerFactory,
        ILogger<PaymentStatusEventConsumer> logger) : base(logger, requestContext)
    {
        _requestContext = requestContext;
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<BillStatusChangedMessage> context)
    {
        var message = context.Message;
        var handler = _handlerFactory.Get<SendPaymentReceiptHandler>();
        await handler.HandleAsync(message);
    }
}