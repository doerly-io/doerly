using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetConversationByIdHandler(CommunicationDbContext dbContext, IProfileService profileService) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<ConversationResponseDto>> HandleAsync(int conversationId)
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
            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
            .AsNoTracking()
            .FirstOrDefaultAsync();
                
        if (conversation == null)
        {
            return HandlerResult.Failure<ConversationResponseDto>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        var participantsIds = conversation.ParticipantIds;
        var profiles = new Dictionary<int, ProfileDto>();
        foreach (var participantId in participantsIds)
        {
            var profile = await profileService.GetProfileAsync(participantId);
            profiles[participantId] = profile;
        }
        
        var conversationResponseDto =  new ConversationResponseDto()
            {
                Id = conversation.Id,
                Initiator = profiles.GetValueOrDefault(conversation.InitiatorId),
                Recipient = profiles.GetValueOrDefault(conversation.RecipientId),
                Messages = conversation.Messages.Select(m => new MessageResponseDto()
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Sender = profiles.GetValueOrDefault(m.SenderId),
                    ConversationId = m.ConversationId,
                    MessageContent = m.MessageContent,
                    SentAt = m.SentAt,
                    Status = m.Status
                }).ToList()
            };

        return HandlerResult.Success(conversationResponseDto);
    }
}