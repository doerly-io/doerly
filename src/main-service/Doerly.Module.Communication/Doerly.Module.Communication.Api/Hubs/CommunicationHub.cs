using System.Security.Claims;
using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Module.Communication.Contracts.Requests;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Domain.Helpers;
using Doerly.Module.Communication.Domain.Hubs;
using Doerly.Module.Communication.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Communication.Api.Hubs;

[Authorize]
public class CommunicationHub(IHandlerFactory handlerFactory, IUserOnlineStatusHelper userOnlineStatusHelper, IDoerlyRequestContext doerlyRequestContext) : Hub<ICommunicationHub>
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
        
        var messageId = await handlerFactory.Get<SendMessageHandler>().HandleAsync(userId, request);
        var message = (await handlerFactory.Get<GetMessageByIdHandler>().HandleAsync(messageId.Value)).Value;
        
        // Send message to conversation group
        await Clients.Group(request.ConversationId.ToString()).ReceiveMessage(message);
        
        // Update last message for both participants
        var participantIds = new[] { message.Conversation.Initiator.Id.ToString(), message.Conversation.Recipient.Id.ToString() };
        await Clients.Users(participantIds).UpdateLastMessage(message);
    }
    
    public async Task SendTyping(string conversationId, string fullName)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await Clients.OthersInGroup(conversationId).UserTyping(fullName);
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
    
    public async Task LeaveConversation(string conversationId)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, false);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }

    #region Message Status Management

    public async Task MarkMessagesAsRead(int[] messageIds, string senderId)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await handlerFactory.Get<MarkMessageStatusHandler>().HandleAsync(messageIds, EMessageStatus.Read);
        
        // Notify about each message status change
        foreach (var messageId in messageIds)
        {
            await Clients.Others.MessageRead(messageId, senderId);
        }
    }

    public async Task MarkMessagesAsDelivered(int[] messageIds, string senderId)
    {
        var userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException();

        await MarkUserStatusAsync(userId, true);
        
        await handlerFactory.Get<MarkMessageStatusHandler>().HandleAsync(messageIds, EMessageStatus.Delivered);
        
        // Notify about each message status change
        foreach (var messageId in messageIds)
        {
            await Clients.Others.MessageDelivered(messageId, senderId);
        }
    }

    #endregion

    #region Connection Management

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

    #endregion
}