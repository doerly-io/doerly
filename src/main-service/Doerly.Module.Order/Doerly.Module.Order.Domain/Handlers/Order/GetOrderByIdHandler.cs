using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using Doerly.Proxy.Profile;
using Doerly.FileRepository;
using FileInfo = Doerly.Module.Order.Contracts.Dtos.FileInfo;
using Doerly.Module.Common.DataAccess.Address;

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
    public async Task<HandlerResult<GetOrderResponse>> HandleAsync(int id)
    {
        var order = await DbContext.Orders
            .Select(order => new GetOrderResponse
            {
                Id = order.Id,
                ServiceId = order.ServiceId,
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
                AddressInfo = new AddressInfo
                {
                    CityId = order.CityId,
                    RegionId = order.RegionId
                },
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
        order.UseProfileAddress = customerProfile.Value.Address?.RegionId == order.AddressInfo.RegionId
            && customerProfile.Value.Address?.CityId == order.AddressInfo.CityId;

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

        return HandlerResult.Success(order);
    }
}
