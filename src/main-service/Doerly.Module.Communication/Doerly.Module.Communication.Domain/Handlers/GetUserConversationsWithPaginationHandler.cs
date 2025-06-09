using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Module.Communication.Contracts.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetUserConversationsWithPaginationHandler(CommunicationDbContext dbContext, IProfileModuleProxy profileModule) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<OperationResult<GetUserConversationsWithPaginationResponse>> HandleAsync(int userId, GetEntitiesWithPaginationRequest pagination)
    {
        var (conversations, totalCount) = await _dbContext.Conversations
            .Where(c => c.InitiatorId == userId || c.RecipientId == userId)
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
            var profile = (await profileModule.GetProfileAsync(participantId)).Value;
            profiles[participantId] = profile;
        }
        
        var conversationHeaderResponseDtos = conversations
            .Select(c => new ConversationHeaderResponse
        {
            Id = c.Id,
            Initiator = profiles.GetValueOrDefault(c.InitiatorId),
            Recipient = profiles.GetValueOrDefault(c.RecipientId),
            LastMessage = c.Messages.Select(m => new MessageResponse()
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Sender = profiles.GetValueOrDefault(m.SenderId),
                ConversationId = m.ConversationId,
                MessageContent = m.MessageContent,
                MessageType = m.MessageType,
                SentAt = m.SentAt,
                Status = m.Status
            }).LastOrDefault()
        }).ToList();
        
        var result = new GetUserConversationsWithPaginationResponse
        {
            Total = totalCount,
            Conversations = conversationHeaderResponseDtos
        };

        return OperationResult.Success(result);
    }
}
