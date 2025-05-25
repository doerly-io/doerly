using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrderByIdHandler : BaseOrderHandler
{
    public GetOrderByIdHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<HandlerResult<GetOrderResponse>> HandleAsync(int id)
    {
        var order = await DbContext.Orders.FindAsync(id);
        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        var orderDto = new GetOrderResponse
        {
            Id = order.Id,
            CategoryId = order.CategoryId,
            Name = order.Name,
            Description = order.Description,
            Price = order.Price,
            PaymentKind = order.PaymentKind,
            DueDate = order.DueDate,
            Status = order.Status,
            CustomerId = order.CustomerId,
            CustomerCompletionConfirmed = order.CustomerCompletionConfirmed,    
            ExecutorId = order.ExecutorId,
            ExecutorCompletionConfirmed = order.ExecutorCompletionConfirmed,
            ExecutionDate = order.ExecutionDate,
            BillId = order.BillId
        };

        return HandlerResult.Success(orderDto);
    }
}
