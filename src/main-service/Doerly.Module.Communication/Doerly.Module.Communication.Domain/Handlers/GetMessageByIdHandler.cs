using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Communication.DataTransferObjects.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.Domain.Constants;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetMessageByIdHandler(CommunicationDbContext dbContext, IProfileModuleProxy profileModule, IFileRepository fileRepository) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<OperationResult<MessageResponse>> HandleAsync(int messageId)
    {
        var message = await _dbContext.Messages
            .AsNoTracking()
            .Where(x => x.Id == messageId)
            .Include(x => x.Conversation)
            .FirstOrDefaultAsync();

        if (message == null)
            return OperationResult.Failure<MessageResponse>(Resources.Get("Communication.MessageNotFound"));
        
        var profiles = new Dictionary<int, ProfileDto>();
        foreach (var userId in message.Conversation.ParticipantIds)
        {
            var profile = (await profileModule.GetProfileAsync(userId)).Value;
            profiles[userId] = profile;
        }
        
        string messageContent;
        if (message.MessageType == EMessageType.Text)
        {
            messageContent = message.MessageContent;
        }
        else
        {
            messageContent = await fileRepository.GetSasUrlAsync(
                CommunicationConstants.AzureStorage.FilesContainerName,
                message.MessageContent
            ) ?? throw new InvalidOperationException("Failed to generate file URL");
        }

        var messageDto = new MessageResponse()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            Conversation = new ConversationHeaderResponse
            {
                Id = message.Conversation.Id,
                Initiator = profiles.GetValueOrDefault(message.Conversation.InitiatorId),
                Recipient = profiles.GetValueOrDefault(message.Conversation.RecipientId),
            },
            MessageContent = messageContent,
            MessageType = message.MessageType,
            SentAt = message.SentAt,
            Status = message.Status
        };
        
        return OperationResult.Success(messageDto);
    }
}
