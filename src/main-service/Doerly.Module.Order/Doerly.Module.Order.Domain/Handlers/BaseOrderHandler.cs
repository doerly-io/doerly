using Doerly.Domain.Handlers;
using Doerly.FileRepository;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;

using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
using Microsoft.AspNetCore.Http;
using Doerly.Module.Profile.DataAccess.Models;
using Doerly.Module.Order.Contracts.Dtos;
using FileInfo = Doerly.Module.Order.Contracts.Dtos.FileInfo;
using Doerly.Module.Order.Contracts.Messages;
using Doerly.Module.Order.Enums;
using Doerly.Messaging;
using Doerly.Domain.Helpers;
using Doerly.Localization;
using Doerly.Domain.Exceptions;
using Doerly.Module.Order.Domain.Constants;
using DoerlyDomain.Constants;

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

    protected async Task<ICollection<OrderFile>> CreateOrderFilesAsync(Guid orderCode, IEnumerable<IFormFile> files)
    {
        var orderFiles = new List<OrderFile>();
        foreach (var file in files)
        {
            var orderFile = await CreateOrderFileAsync(orderCode, file);
            if (orderFile != null)
                orderFiles.Add(orderFile);
        }
        return orderFiles;
    }

    protected async Task<OrderFile> CreateOrderFileAsync(Guid orderCode, IFormFile file)
    {
        if (file.Length == 0)
            return null;

        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        var fileBytes = memoryStream.ToArray();

        if (!IsValidImage(fileBytes))
            return null;

        var filePath = $"{AzureStorageConstants.FolderNames.OrderImages}/{orderCode}/{file.FileName}";

        await UploadOrderFile(filePath, fileBytes);

        return new OrderFile
        {
            Path = filePath,
            Name = file.FileName,
            Size = file.Length,
            Type = file.ContentType
        };
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

    protected async Task DeleteOrderFileFromStorageAsync(OrderFile file)
    {
        await _fileRepository.DeleteFileIfExistsAsync(
            AzureStorageConstants.ImagesContainerName,
            file.Path);
    }

    protected async Task UploadOrderFile(string filePath, byte[] fileBytes) 
    {
        await _fileRepository.UploadFileAsync(
            AzureStorageConstants.ImagesContainerName,
            filePath,
            fileBytes);
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

    private bool IsValidImage(byte[] fileBytes)
    {
        if (ImageValidationHelper.IsValidImage(fileBytes, out var fileExtension)
            && ImageExtensions.SupportedExtensions.Any(extension => extension.Contains(fileExtension)))
            return true;
        return false;
    }
}
