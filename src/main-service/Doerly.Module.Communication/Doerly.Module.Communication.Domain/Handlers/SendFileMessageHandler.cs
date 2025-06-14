using System.Text.Json;
using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Messaging;
using Doerly.Module.Communication.DataTransferObjects.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.DataTransferObjects.Messages;
using Doerly.Module.Communication.Domain.Constants;
using Doerly.Module.Communication.Enums;
using Doerly.Proxy.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendFileMessageHandler(CommunicationDbContext dbContext, 
    IProfileModuleProxy profileModuleProxy,
    IMessagePublisher messagePublisher,
    IFileRepository fileRepository)
    : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<OperationResult<int>> HandleAsync(int conversationId, int userId, byte[] fileBytes, string fileName)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);
        
        if (conversation == null)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        if (!FileHelper.IsValidFile(fileName, fileBytes, CommunicationConstants.FileConstants.SupportedFileExtensions, CommunicationConstants.FileConstants.MaxFileSizeInBytes))
        {
            return OperationResult.Failure<int>(Resources.Get("InvalidDocument"));
        }
        
        if (fileBytes.Length == 0)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.FileCannotBeEmpty"));
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.MessageContentCannotBeEmpty"));
        }
    
        //TODO: change filePath to save file name
        var filePath = $"{CommunicationConstants.FolderNames.CommunicationFiles}/{conversationId}/{Guid.NewGuid()}{FileHelper.GetFileExtension(fileName)}";
        await fileRepository.UploadFileAsync(
            CommunicationConstants.AzureStorage.FilesContainerName, 
            filePath, 
            fileBytes);
        
        var message = new MessageEntity
        {
            ConversationId = conversation.Id,
            SenderId = userId,
            MessageContent = filePath,
            SentAt = DateTime.UtcNow,
            Status = EMessageStatus.Sent,
            MessageType = EMessageType.File
        };
    
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
        
        // Get sender's name for the notification
        var sender = await profileModuleProxy.GetProfileAsync(userId);
        if (!sender.IsSuccess)
        {
            return OperationResult.Failure<int>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        // Notify the conversation participants about the new message
        var notificationMessage = new NewMessageNotificationMessage(
            conversation.RecipientId == userId ? conversation.InitiatorId : conversation.RecipientId,
            JsonSerializer.Serialize(new { 
                conversationId = conversation.Id, 
                messageId = message.Id,
                senderName = $"{sender.Value.FirstName} {sender.Value.LastName}",
            }),
            message.SentAt
        );
        await messagePublisher.Publish(notificationMessage);
        
        return OperationResult.Success(message.Id);
    }
}