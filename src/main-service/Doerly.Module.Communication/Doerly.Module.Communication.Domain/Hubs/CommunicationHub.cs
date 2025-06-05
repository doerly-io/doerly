using System.Security.Claims;
using Doerly.Domain.Factories;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Communication.Domain.Hubs;

[Authorize]
public class CommunicationHub(IHandlerFactory handlerFactory) : Hub<ICommunicationHub>
{
    public async Task SendMessage(SendMessageRequest request)
    {
        var userIdString = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = int.TryParse(userIdString, out var id) ? id : throw new UnauthorizedAccessException();
        var result = await handlerFactory.Get<SendMessageHandler>().HandleAsync(userId, request);
        await Clients.Group(request.ConversationId.ToString()).ReceiveMessage(result.Value);
    }
    
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Group(conversationId).UserJoined(conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Caller.LeaveConversation(conversationId);
    }
    
    public async Task SendTyping(string conversationId, string fullName)
    {
        await Clients.OthersInGroup(conversationId).UserTyping(fullName);
    }
    
    public async Task MarkMessageAsRead(string conversationId, int messageId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await Clients.Group(conversationId).MessageRead(messageId, userId);
    }

    public async Task UpdateUserStatus(string conversationId, bool isOnline)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await Clients.Group(conversationId).UserStatusChanged(userId, isOnline);
    }
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}