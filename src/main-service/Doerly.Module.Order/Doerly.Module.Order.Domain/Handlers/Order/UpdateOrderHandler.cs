using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;
using Doerly.Domain;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateOrderHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public UpdateOrderHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id, UpdateOrderRequest dto)
    {
        var order = await DbContext.Orders.Select(x => new { x.Id, x.CustomerId })
            .FirstOrDefaultAsync(x => x.Id == id && x.CustomerId == _doerlyRequestContext.UserId);
        
        if (order == null)
            return HandlerResult.Failure<GetOrderResponse>(Resources.Get("OrderNotFound"));

        await DbContext.Orders.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(order => order.CategoryId, dto.CategoryId)
            .SetProperty(order => order.Name, dto.Name)
            .SetProperty(order => order.Description, dto.Description)
            .SetProperty(order => order.Price, dto.Price)
            .SetProperty(order => order.PaymentKind, dto.PaymentKind)
            .SetProperty(order => order.DueDate, dto.DueDate));

        return HandlerResult.Success();
    }
}
