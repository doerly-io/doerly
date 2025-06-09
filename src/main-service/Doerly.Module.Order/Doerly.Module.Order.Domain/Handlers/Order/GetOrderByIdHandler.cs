using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;
using Doerly.Proxy.Profile;
using Doerly.FileRepository;
using FileInfo = Doerly.Module.Order.DataTransferObjects.FileInfo;
using Doerly.Module.Common.DataAccess.Address;
using Doerly.Module.Order.DataTransferObjects;

namespace Doerly.Module.Order.Domain.Handlers;

public class GetOrderByIdHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;
    private readonly AddressDbContext _addressDbContext;

    public GetOrderByIdHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy, AddressDbContext addressDbContext,
        IFileRepository fileRepository) : base(dbContext, fileRepository)
    {
        _profileModuleProxy = profileModuleProxy;
        _addressDbContext = addressDbContext;
    }
    public async Task<OperationResult<GetOrderResponse>> HandleAsync(int id)
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
                UseProfileAddress = order.UseProfileAddress,
                AddressInfo = new AddressInfo
                {
                    CityId = order.CityId,
                    RegionId = order.RegionId
                },
                Feedback = order.Feedback != null ? new OrderFeedbackResponse
                {
                    UserProfile = new ProfileInfo
                    {
                        UserId = order.Feedback != null ? order.Feedback.ReviewerUserId : -1,
                    },
                    FeedbackId = order.Feedback.Id,
                    Comment = order.Feedback.Comment,
                    Rating = order.Feedback.Rating,
                    CreatedAt = order.Feedback.DateCreated,
                    UpdatedAt = order.Feedback.DateCreated == order.Feedback.LastModifiedDate ? null : order.Feedback.LastModifiedDate,
                } : null,
                ExistingFiles = order.OrderFiles.Select(file => new FileInfo
                {
                    FilePath = file.Path,
                    FileName = file.Name,
                    FileSize = file.Size,
                    FileType = file.Type
                }),
                CreatedDate = order.DateCreated
            })
            .FirstOrDefaultAsync(order => order.Id == id);

        if (order == null)
            return OperationResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        await SetOrderFileUrls(order.ExistingFiles);

        var profileIds = new List<int>(2) { order.CustomerId };
        if (order.Feedback?.UserProfile is { UserId: > 0 })
            profileIds.Add(order.Feedback.UserProfile.Id);

        var profiles = await _profileModuleProxy.GetProfilesShortInfoWithAvatarAsync(profileIds);

        var customerProfile = profiles.First(x => x.Id == order.CustomerId);
        order.Customer = new ProfileInfo
        {
            Id = customerProfile.Id,
            FirstName = customerProfile.FirstName,
            LastName = customerProfile.LastName,
            AvatarUrl = customerProfile.AvatarUrl
        };

        var reviewerProfile = profiles
            .FirstOrDefault(x => x.UserId == order.Feedback?.UserProfile.UserId);
        if (order.Feedback != null && reviewerProfile != null)
        {
            order.Feedback.UserProfile = new ProfileInfo
            {
                Id = order.Feedback.UserProfile.Id,
                FirstName = reviewerProfile.FirstName,
                LastName = reviewerProfile.LastName,
                AvatarUrl = reviewerProfile.AvatarUrl
            };
        }

        var address = await _addressDbContext.Cities.AsNoTracking()
            .Select(city => new AddressInfo
            {
                CityName = city.Name,
                CityId = city.Id,
                RegionName = city.Region.Name,
                RegionId = city.Region.Id
            })
            .FirstOrDefaultAsync(x => x.CityId == order.AddressInfo.CityId && x.RegionId == order.AddressInfo.RegionId);

        order.AddressInfo = address;

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

        return OperationResult.Success(order);
    }
}
