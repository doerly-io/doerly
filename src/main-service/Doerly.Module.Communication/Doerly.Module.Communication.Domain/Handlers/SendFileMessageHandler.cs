using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Constants;
using Doerly.Module.Communication.Domain.Hubs;
using Doerly.Module.Communication.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendFileMessageHandler(CommunicationDbContext dbContext, 
    IFileRepository fileRepository, 
    IFileHelper fileHelper,
    IHubContext<CommunicationHub, ICommunicationHub> communicationHub) : BaseCommunicationHandler(dbContext)
{
    public async Task<HandlerResult> HandleAsync(int conversationId, int userId, IFormFile file)
    {
        var conversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);
        
        if (conversation == null)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        var fileBytes = await fileHelper.GetFormFileBytesAsync(file);
        if (!fileHelper.IsValidFile(file.FileName, fileBytes, CommunicationConstants.FileConstants.SupportedFileExtensions, CommunicationConstants.FileConstants.MaxFileSizeInBytes))
        {
            return HandlerResult.Failure(Resources.Get("InvalidDocument"));
        }
    
        //TODO: change filePath to save file name
        var filePath = $"{CommunicationConstants.FolderNames.CommunicationFiles}/{conversationId}/{Guid.NewGuid()}{fileHelper.GetFileExtension(file.FileName)}";
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
            Status = MessageStatus.Sent,
            MessageType = EMessageType.File
        };
    
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
        
        var response = new MessageResponseDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            MessageContent = await fileRepository.GetSasUrlAsync(CommunicationConstants.AzureStorage.FilesContainerName, filePath) ?? throw new InvalidOperationException(),
            SentAt = message.SentAt,
            Status = message.Status,
            MessageType = EMessageType.File
        };
        
        // Notify clients in the conversation about the new message
        await communicationHub.Clients.Group(conversation.Id.ToString()).ReceiveMessage(response);
        return HandlerResult.Success();
    }
}