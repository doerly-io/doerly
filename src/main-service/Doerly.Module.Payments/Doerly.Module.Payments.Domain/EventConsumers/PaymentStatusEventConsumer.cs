using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Payments.DataTransferObjects.Messages;
using Doerly.Module.Payments.Domain.Handlers;
using Doerly.Module.Payments.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.EventConsumers;

public class PaymentStatusEventConsumer : BaseConsumer<BillStatusChangedMessage>
{
    private readonly IDoerlyRequestContext _requestContext;
    private readonly IHandlerFactory _handlerFactory;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PaymentStatusEventConsumer(
        IDoerlyRequestContext requestContext,
        IHandlerFactory handlerFactory,
        IWebHostEnvironment webHostEnvironment,
        ILogger<PaymentStatusEventConsumer> logger) : base(logger, requestContext)
    {
        _requestContext = requestContext;
        _handlerFactory = handlerFactory;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override async Task Handle(ConsumeContext<BillStatusChangedMessage> context)
    {
        if (_webHostEnvironment.IsDevelopment())
            return;

        var message = context.Message;
        var handler = _handlerFactory.Get<SendPaymentReceiptHandler>();
        await handler.HandleAsync(message);
    }
}
