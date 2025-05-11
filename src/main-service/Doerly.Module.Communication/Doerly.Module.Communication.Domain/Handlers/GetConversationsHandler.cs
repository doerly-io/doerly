using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class GetConversationsHandler(CommunicationDbContext dbContext) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<List<ConversationHeaderResponseDto>>> HandleAsync(int userId, int pageNumber, int pageSize)
    {
        var conversations = await _dbContext.Conversations
            .Where(c => c.InitiatorId == userId)
            .OrderByDescending(c => c.LastModifiedDate)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var conversationHeaderResponseDtos = conversations
            .Select(c => new ConversationHeaderResponseDto
        {
            Id = c.Id,
            InitiatorId = c.InitiatorId,
            RecipientId = c.RecipientId,
        }).ToList();

        return HandlerResult.Success(conversationHeaderResponseDtos);
    }
}