using Doerly.Messaging;
using Doerly.Module.Payments.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.EventConsumers;

public class PaymentStatusEventConsumer : BaseConsumer<BillStatusChangedMessage>
{
    public PaymentStatusEventConsumer(ILogger logger) : base(logger)
    {
    }

    protected override Task Handle(ConsumeContext<BillStatusChangedMessage> context)
    {
        //ToDo: add sending email notification logic here
        return Task.CompletedTask;
    }
}
