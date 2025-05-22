using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetUserConversationsWithPaginationHandler(CommunicationDbContext dbContext, IProfileService profileService) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<GetUserConversationsWithPaginationResponse>> HandleAsync(int userId, GetEntitiesWithPaginationRequest pagination)
    {
        var (conversations, totalCount) = await _dbContext.Conversations
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
            .GetEntitiesWithPaginationAsync(pagination.PageInfo);
        
        var participantsIds = conversations.SelectMany(c => c.ParticipantIds).Distinct().ToList();
        var profiles = new Dictionary<int, ProfileDto>();
        foreach (var participantId in participantsIds)
        {
            var profile = await profileService.GetProfileAsync(participantId);
            profiles[participantId] = profile;
        }
        
        var conversationHeaderResponseDtos = conversations
            .Select(c => new ConversationHeaderResponseDto
        {
            Id = c.Id,
            Initiator = profiles.GetValueOrDefault(c.InitiatorId),
            Recipient = profiles.GetValueOrDefault(c.RecipientId),
            LastMessage = c.Messages.Select(m => new MessageResponseDto()
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Sender = profiles.GetValueOrDefault(m.SenderId),
                ConversationId = m.ConversationId,
                MessageContent = m.MessageContent,
                SentAt = m.SentAt,
                Status = m.Status
            }).Last()
        }).ToList();
        
        var result = new GetUserConversationsWithPaginationResponse
        {
            Total = totalCount,
            Conversations = conversationHeaderResponseDtos
        };

        return HandlerResult.Success(result);
    }
}