using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Communication.Domain.Hubs;

[Authorize]
public class CommunicationHub : Hub<ICommunicationHub>
{
    public async Task SendMessage(string conversationId, string senderId, string messageContent)
    {
        await Clients.Group(conversationId).SendMessage(senderId, messageContent);
    }
    
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Group(conversationId).JoinConversation(conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Caller.LeaveConversation(conversationId);
    }
    
    public async Task SendTyping(string conversationId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await Clients.OthersInGroup(conversationId).UserTyping(userId);
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
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // await Clients.All.UserStatusChanged(userId, true);
    }
}