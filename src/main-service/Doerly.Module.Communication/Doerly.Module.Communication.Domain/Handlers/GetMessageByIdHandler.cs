using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetMessageByIdHandler(CommunicationDbContext dbContext, IProfileService profileService) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<MessageResponseDto>> HandleAsync(int messageId)
    {
        var message = await _dbContext.Messages
            .AsNoTracking()
            .Where(x => x.Id == messageId)
            .Include(x => x.Conversation)
            .FirstOrDefaultAsync();

        if (message == null)
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.MessageNotFound"));
        
        var profiles = new Dictionary<int, ProfileDto>();
        foreach (var userId in message.Conversation.ParticipantIds)
        {
            var profile = await profileService.GetProfileAsync(userId);
            profiles[userId] = profile;
        }
        
        var messageDto = new MessageResponseDto()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            Conversation = new ConversationHeaderResponseDto
            {
                Id = message.Conversation.Id,
                Initiator = profiles.GetValueOrDefault(message.Conversation.InitiatorId),
                Recipient = profiles.GetValueOrDefault(message.Conversation.RecipientId),
            },
            MessageContent = message.MessageContent,
            SentAt = message.SentAt,
            Status = message.Status
        };

        return HandlerResult.Success(messageDto);
    }
}