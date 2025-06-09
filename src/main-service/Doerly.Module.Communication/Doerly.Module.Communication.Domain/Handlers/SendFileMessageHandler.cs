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
        
        return OperationResult.Success(message.Id);
    }
}