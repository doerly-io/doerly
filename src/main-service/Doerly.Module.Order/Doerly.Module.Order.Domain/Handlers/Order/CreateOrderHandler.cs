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

namespace Doerly.Module.Order.Domain.Handlers;

public class CreateOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public CreateOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext,
        IFileRepository fileRepository, IMessagePublisher messagePublisher) : base(dbContext, fileRepository, messagePublisher)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest dto, List<IFormFile> files)
    {
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
            CustomerId = _doerlyRequestContext.UserId ?? throw new DoerlyException("We are fucked!"),
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
