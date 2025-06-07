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
    IHubContext<CommunicationHub, ICommunicationHub> communicationHub) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult> HandleAsync(int conversationId, int userId, IFormFile file)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);
        
        if (conversation == null)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return HandlerResult.Failure<MessageResponseDto>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        var fileBytes = await FileHelper.GetFormFileBytesAsync(file);
        if (!FileHelper.IsValidFile(file.FileName, fileBytes, CommunicationConstants.FileConstants.SupportedFileExtensions, CommunicationConstants.FileConstants.MaxFileSizeInBytes))
        {
            return HandlerResult.Failure(Resources.Get("InvalidDocument"));
        }
    
        //TODO: change filePath to save file name
        var filePath = $"{CommunicationConstants.FolderNames.CommunicationFiles}/{conversationId}/{Guid.NewGuid()}{FileHelper.GetFileExtension(file.FileName)}";
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