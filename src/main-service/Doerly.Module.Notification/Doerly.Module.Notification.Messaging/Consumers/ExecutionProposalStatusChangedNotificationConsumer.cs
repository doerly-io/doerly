using System.Text.Json;
using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using Doerly.Module.Order.DataTransferObjects.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Notification.Messaging.Consumers;

public class ExecutionProposalStatusChangedNotificationConsumer(
    ILogger<ExecutionProposalStatusChangedNotificationConsumer> logger,
    IDoerlyRequestContext doerlyRequestContext,
    IHandlerFactory handlerFactory)
    : BaseConsumer<ExecutionProposalStatusChangedMessage>(logger, doerlyRequestContext)
{
    protected override Task Handle(ConsumeContext<ExecutionProposalStatusChangedMessage> context)
    {
        return handlerFactory.Get<SendNotificationHandler>().HandleAsync(
            context.Message.ReceiverId,
            NotificationConstants.ExecutionProposals.StatusChanged,
            NotificationType.ExecutionProposal,
            DateTime.UtcNow, 
            JsonSerializer.Serialize(new { executionProposalId = context.Message.ExecutionProposalId, executionProposalStatus = context.Message.ExecutionProposalStatus })
        );
    }
} 