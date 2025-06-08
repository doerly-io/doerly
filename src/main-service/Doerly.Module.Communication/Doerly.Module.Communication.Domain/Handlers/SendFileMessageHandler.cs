using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Localization;
using Doerly.Module.Communication.Contracts.Responses;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.DataAccess.Entities;
using Doerly.Module.Communication.Domain.Constants;
using Doerly.Module.Communication.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Communication.Domain.Handlers;

public class SendFileMessageHandler(CommunicationDbContext dbContext, 
    IFileRepository fileRepository) : BaseCommunicationHandler(dbContext)
{
    private readonly CommunicationDbContext _dbContext = dbContext;

    public async Task<HandlerResult<int>> HandleAsync(int conversationId, int userId, IFormFile file)
    {
        var conversation = await _dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);
        
        if (conversation == null)
        {
            return HandlerResult.Failure<int>(Resources.Get("Communication.ConversationNotFound"));
        }
        
        if (conversation.InitiatorId != userId && conversation.RecipientId != userId)
        {
            return HandlerResult.Failure<int>(Resources.Get("Communication.UnauthorizedSender"));
        }
        
        var fileBytes = await FileHelper.GetFormFileBytesAsync(file);
        if (!FileHelper.IsValidFile(file.FileName, fileBytes, CommunicationConstants.FileConstants.SupportedFileExtensions, CommunicationConstants.FileConstants.MaxFileSizeInBytes))
        {
            return HandlerResult.Failure<int>(Resources.Get("InvalidDocument"));
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
        
        return HandlerResult.Success(message.Id);
    }
}