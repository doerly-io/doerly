using Doerly.Domain.Exceptions;
using Doerly.Domain.Handlers;
using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Helpers;
using Doerly.Localization;
using Doerly.Messaging;
using Doerly.Module.Order.DataTransferObjects.Messages;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Constants;
using Doerly.Module.Order.Enums;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using DoerlyDomain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FileInfo = Doerly.Module.Order.DataTransferObjects.FileInfo;
using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;

namespace Doerly.Module.Order.Domain.Handlers;

public class BaseOrderHandler : BaseHandler<OrderDbContext>
{
    private readonly IFileRepository _fileRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IProfileModuleProxy _profileModuleProxy;

    public BaseOrderHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }

    public BaseOrderHandler(OrderDbContext dbContext, IFileRepository fileRepository) : base(dbContext)
    {
        _fileRepository = fileRepository;
    }

    public BaseOrderHandler(OrderDbContext dbContext, IFileRepository fileRepository, IProfileModuleProxy profileModuleProxy) :
        base(dbContext)
    {
        _fileRepository = fileRepository;
        _profileModuleProxy = profileModuleProxy;
    }

    public BaseOrderHandler(OrderDbContext dbContext, IMessagePublisher messagePublisher) : base(dbContext)
    {
        _messagePublisher = messagePublisher;
    }

    public BaseOrderHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy, IFileRepository fileRepository,
        IMessagePublisher messagePublisher) : base(dbContext)
    {
        _profileModuleProxy = profileModuleProxy;
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
        await file.CopyToAsync(memoryStream);
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

    protected async Task PublishExecutionProposalStatusUpdatedEventAsync(int executionProposalId,
        EExecutionProposalStatus executionProposalStatus)
    {
        var message = new ExecutionProposalStatucUpdatedMessage(executionProposalId, executionProposalStatus);
        await _messagePublisher.Publish(message);
    }

    protected async Task<(int regionId, int cityId)> ManageAddress(int userId, bool useProfileAddress, int? dtoRegionId, int? dtoCityId)
    {
        ProfileDto profile;
        int regionId = 0;
        int cityId = 0;
        if (useProfileAddress)
        {
            profile = (await _profileModuleProxy.GetProfileAsync(userId))?.Value;
            if (profile?.Address == null)
                throw new DoerlyException(Resources.Get("AddressIsEmpty"));
            else
            {
                regionId = profile.Address.RegionId;
                cityId = profile.Address.CityId;
            }
        }
        else if (dtoRegionId == null || dtoCityId == null)
            throw new DoerlyException(Resources.Get("AddressIsEmpty"));
        else
        {
            regionId = (int)dtoRegionId;
            cityId = (int)dtoCityId;
        }

        return (regionId, cityId);
    }

    protected async Task<OperationResult<SendExecutionProposalResponse>> SendExecutionProposal(SendExecutionProposalRequest dto, int userId)
    {
        if (dto.ReceiverId == userId)
            throw new DoerlyException(Resources.Get("InvalidUserForProposal"));

        var executionProposal = new ExecutionProposal()
        {
            OrderId = dto.OrderId,
            Comment = dto.Comment.Trim(),
            SenderId = userId,
            ReceiverId = dto.ReceiverId,
            Status = EExecutionProposalStatus.Pending
        };

        DbContext.ExecutionProposals.Add(executionProposal);
        await DbContext.SaveChangesAsync();

        await PublishExecutionProposalStatusUpdatedEventAsync(executionProposal.Id, executionProposal.Status);

        var result = new SendExecutionProposalResponse()
        {
            Id = executionProposal.Id
        };

        return OperationResult.Success(result);
    }

    private bool IsValidImage(byte[] fileBytes)
    {
        if (ImageValidationHelper.IsValidImage(fileBytes, out var fileExtension)
            && ImageExtensions.SupportedExtensions.Any(extension => extension.Contains(fileExtension)))
            return true;
        return false;
    }
}
