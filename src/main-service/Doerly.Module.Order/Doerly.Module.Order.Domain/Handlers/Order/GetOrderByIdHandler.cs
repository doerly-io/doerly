using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using Doerly.Proxy.Profile;
using Doerly.FileRepository;
using FileInfo = Doerly.Module.Order.Contracts.Dtos.FileInfo;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrderByIdHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public GetOrderByIdHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy,
        IFileRepository fileRepository) : base(dbContext, fileRepository)
    {
        _profileModuleProxy = profileModuleProxy;
    }
    public async Task<HandlerResult<GetOrderResponse>> HandleAsync(int id)
    {
        var order = await DbContext.Orders
            .Select(order => new GetOrderResponse
            {
                Id = order.Id,
                CategoryId = order.CategoryId,
                Name = order.Name,
                Description = order.Description,
                Price = order.Price,
                IsPriceNegotiable = order.IsPriceNegotiable,
                PaymentKind = order.PaymentKind,
                DueDate = order.DueDate,
                Status = order.Status,
                CustomerId = order.CustomerId,
                CustomerCompletionConfirmed = order.CustomerCompletionConfirmed,
                ExecutorId = order.ExecutorId,
                ExecutorCompletionConfirmed = order.ExecutorCompletionConfirmed,
                ExecutionDate = order.ExecutionDate,
                BillId = order.BillId,
                ExistingFiles = order.OrderFiles.Select(file => new FileInfo
                {
                    FilePath = file.FilePath,
                    FileName = file.FileName,
                    FileSize = file.FileSize,
                    FileType = file.FileType
                }),
                CreatedDate = order.DateCreated
            })
            .FirstOrDefaultAsync(order => order.Id == id);

        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        await SetOrderFileUrls(order.ExistingFiles);

        var customerProfile = await _profileModuleProxy.GetProfileAsync(order.CustomerId);
        order.Customer = new ProfileInfo
        {
            Id = customerProfile.Value.Id,
            FirstName = customerProfile.Value.FirstName,
            LastName = customerProfile.Value.LastName,
            AvatarUrl = customerProfile.Value.ImageUrl
        };

        if (order.ExecutorId.HasValue)
        {
            var executorProfile = await _profileModuleProxy.GetProfileAsync(order.ExecutorId.Value);
            order.Executor = new ProfileInfo
            {
                Id = executorProfile.Value.Id,
                FirstName = executorProfile.Value.FirstName,
                LastName = executorProfile.Value.LastName,
                AvatarUrl = executorProfile.Value.ImageUrl
            };
        }

        return HandlerResult.Success(order);
    }
}
