using Doerly.Domain.Handlers;
using Doerly.FileRepository;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Profile.Domain.Constants;

using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
using Microsoft.AspNetCore.Http;
using Doerly.Module.Profile.DataAccess.Models;
using Doerly.Module.Order.Contracts.Dtos;
using FileInfo = Doerly.Module.Order.Contracts.Dtos.FileInfo;
using Doerly.Module.Order.Contracts.Messages;
using Doerly.Module.Order.Enums;
using Doerly.Messaging;

namespace Doerly.Module.Order.Domain.Handlers;

public class BaseOrderHandler : BaseHandler<OrderDbContext>
{
    private readonly IFileRepository _fileRepository;
    private readonly IMessagePublisher _messagePublisher;

    public BaseOrderHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public BaseOrderHandler(OrderDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }

    public BaseOrderHandler(OrderDbContext dbContext, IMessagePublisher messagePublisher) : base(dbContext)
    {
        _messagePublisher = messagePublisher;
    }

    public BaseOrderHandler(OrderDbContext dbContext, IFileRepository fileRepository, IMessagePublisher messagePublisher) : base(dbContext)
    {
        _fileRepository = fileRepository;
        _messagePublisher = messagePublisher;
    }

    protected async Task UploadOrderFilesAsync(OrderEntity order, IEnumerable<IFormFile> files)
    {
        foreach (var file in files)
        {
            if (file.Length == 0 || !file.ContentType.Trim().Contains("image", StringComparison.OrdinalIgnoreCase))
                continue;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var filePath = $"{AzureStorageConstants.FolderNames.OrderImages}/{order.Id}/{file.FileName}";

            await _fileRepository.UploadFileAsync(
                AzureStorageConstants.ImagesContainerName,
                filePath,
                fileBytes);

            order.OrderFiles.Add(new OrderFile
            {
                FilePath = filePath,
                FileName = file.FileName,
                FileSize = file.Length,
                FileType = file.ContentType
            });
        }
    }

    protected async Task SetOrderFileUrls(IEnumerable<FileInfo> files)
    {
        foreach (var file in files)
        {
            var fileUrl = await _fileRepository.GetSasUrlAsync(
                                AzureStorageConstants.ImagesContainerName,
                                file.FilePath);
            if (!string.IsNullOrWhiteSpace(fileUrl))
                file.FilePath = fileUrl;
        }
    }

    protected async Task DeleteOrderFilesAsync(OrderEntity order, IEnumerable<OrderFile> files)
    {
        foreach (var existingFile in files)
        {
            await _fileRepository.DeleteFileIfExistsAsync(
                AzureStorageConstants.ImagesContainerName,
                existingFile.FilePath);

            order.OrderFiles.Remove(existingFile);
        }
    }

    protected async Task PublishOrderStatusUpdatedEventAsync(int orderId, EOrderStatus orderStatus)
    {
        var message = new OrderStatusUpdatedMessage(orderId, orderStatus);
        await _messagePublisher.Publish(message);
    }

    protected async Task PublishExecutionProposalStatusUpdatedEventAsync(int executionProposalId, EExecutionProposalStatus executionProposalStatus)
    {
        var message = new ExecutionProposalStatucUpdatedMessage(executionProposalId, executionProposalStatus);
        await _messagePublisher.Publish(message);
    }
}
