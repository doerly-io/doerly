using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.Domain.Constants;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetConversationByIdHandler(CommunicationDbContext dbContext, IProfileModuleProxy profileModule, IFileRepository fileRepository) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<ConversationResponse>> HandleAsync(int conversationId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .Select(c => new
            {
                Conversation = c,
                LastMessageSentAt = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => m.SentAt)
                    .FirstOrDefault()
            })
            .OrderByDescending(c => c.LastMessageSentAt)
            .Select(c => c.Conversation)
            .Include(c => c.Messages.OrderBy(m => m.SentAt))
            .AsNoTracking()
            .FirstOrDefaultAsync();
                
        if (conversation == null)
        {
            return HandlerResult.Failure<ConversationResponse>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        var participantsIds = conversation.ParticipantIds;
        var profiles = new Dictionary<int, ProfileDto>();
        foreach (var participantId in participantsIds)
        {
            var profile = (await profileModule.GetProfileAsync(participantId)).Value;
            profiles[participantId] = profile;
        }
        
        var messageDtos = await Task.WhenAll(conversation.Messages.Select(async m => new MessageResponse
        {
            Id = m.Id,
            SenderId = m.SenderId,
            Sender = profiles.GetValueOrDefault(m.SenderId),
            ConversationId = m.ConversationId,
            MessageContent = m.MessageType == EMessageType.Text
                ? m.MessageContent
                : await fileRepository.GetSasUrlAsync(CommunicationConstants.AzureStorage.FilesContainerName, m.MessageContent)
                  ?? throw new InvalidOperationException(),
            SentAt = m.SentAt,
            MessageType = m.MessageType,
            Status = m.Status
        }));
        
        var conversationResponseDto =  new ConversationResponse()
        {
            Id = conversation.Id,
            Initiator = profiles.GetValueOrDefault(conversation.InitiatorId),
            Recipient = profiles.GetValueOrDefault(conversation.RecipientId),
            Messages = messageDtos.ToList()
        };

        return HandlerResult.Success(conversationResponseDto);
    }
}