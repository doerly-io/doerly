using System.Security.Claims;
using Doerly.Domain.Factories;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Communication.Domain.Hubs;

[Authorize]
public class CommunicationHub(IHandlerFactory handlerFactory, IUserOnlineStatusHelper userOnlineStatusHelper) : Hub<ICommunicationHub>
{
    private async Task MarkUserStatusAsync(int userId, bool isOnline)
    {
        await userOnlineStatusHelper.MarkUserOfflineAsync(userId);
        await Clients.All.UserStatusChanged(userId.ToString(), isOnline);
    }
    
    public async Task SendMessage(SendMessageRequest request)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        var result = await handlerFactory.Get<SendMessageHandler>().HandleAsync(userId, request);
        await Clients.Group(request.ConversationId.ToString()).ReceiveMessage(result.Value);
    }
    
    public async Task JoinConversation(string conversationId)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Group(conversationId).UserJoined(conversationId);
    }
    
    public async Task SendTyping(string conversationId, string fullName)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await Clients.OthersInGroup(conversationId).UserTyping(fullName);
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, false);
        
        //TODO: Implement user last seen update logic
        // var user = await _dbContext.Users.FindAsync(parsedId);
        // if (user != null)
        // {
        //     user.LastSeenAt = DateTime.UtcNow;
        //     await _dbContext.SaveChangesAsync();
        // }

        await base.OnDisconnectedAsync(exception);
    }
}