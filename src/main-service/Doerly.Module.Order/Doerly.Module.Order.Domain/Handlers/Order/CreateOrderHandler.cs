using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Payments.Api.ModuleWrapper;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Enums;

using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
using Doerly.Localization;
using Doerly.Domain;
using Doerly.FileRepository;
using Microsoft.AspNetCore.Http;
using Doerly.Module.Profile.Domain.Constants;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.Contracts.Messages;
using Doerly.Messaging;
using Doerly.Domain.Helpers;
using Doerly.Domain.Exceptions;
using Doerly.Proxy.Profile;
using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Order.Domain.Handlers;

public class CreateOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public CreateOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext, IProfileModuleProxy profileModuleProxy,
        IFileRepository fileRepository, IMessagePublisher messagePublisher) : base(dbContext, profileModuleProxy, fileRepository, messagePublisher)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest dto, List<IFormFile> files)
    {
        var userId = _doerlyRequestContext.UserId ?? throw new DoerlyException("We are fucked!");

        (int regionId, int cityId) = await ManageAddress(userId, dto.UseProfileAddress, dto.RegionId, dto.CityId);

        var orderCode = new Guid();
        ICollection<OrderFile> orderFiles = [];

        if (files != null || files.Count != 0)
            orderFiles = await CreateOrderFilesAsync(orderCode, files);

        var order = new OrderEntity()
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            IsPriceNegotiable = dto.IsPriceNegotiable,
            PaymentKind = dto.PaymentKind,
            DueDate = dto.DueDate,
            Status = EOrderStatus.Placed,
            OrderFiles = orderFiles,
            CustomerId = userId,
            RegionId = regionId,
            CityId = cityId
        };

        DbContext.Orders.Add(order);
        await DbContext.SaveChangesAsync();

        var result = new CreateOrderResponse()
        {
            Id = order.Id
        };

        await PublishOrderStatusUpdatedEventAsync(order.Id, order.Status);

        return HandlerResult.Success(result);
    }
}
