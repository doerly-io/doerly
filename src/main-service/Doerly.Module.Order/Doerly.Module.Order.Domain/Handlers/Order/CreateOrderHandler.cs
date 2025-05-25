using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Payments.Api.ModuleWrapper;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Enums;

using OrderEntity = Doerly.Module.Order.DataAccess.Models.Order;
using Doerly.Localization;

namespace Doerly.Module.Order.Domain.Handlers;

public class CreateOrderHandler : BaseOrderHandler
{
    public CreateOrderHandler(OrderDbContext dbContext) : base(dbContext)
    {}

    public async Task<HandlerResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest dto)
    {
        var order = new OrderEntity()
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            PaymentKind = dto.PaymentKind,
            DueDate = dto.DueDate,
            Status = EOrderStatus.Placed,
            CustomerId = dto.CustomerId,
        };

        DbContext.Orders.Add(order);
        await DbContext.SaveChangesAsync();

        var result = new CreateOrderResponse()
        {
            Id = order.Id
        };

        return HandlerResult.Success(result);
    }
}
