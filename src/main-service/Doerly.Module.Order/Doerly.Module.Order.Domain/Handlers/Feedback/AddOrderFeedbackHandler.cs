using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Module.Order.Contracts.Dtos.Requests;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class AddOrderFeedbackHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public AddOrderFeedbackHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int orderId, AddOrderFeedbackRequest request)
    {
        var orderExists = await DbContext.Orders.AnyAsync(o => o.Id == orderId);
        if (!orderExists)
            return HandlerResult.Failure("ORDER_NOT_FOUND");

        var feedback = new OrderFeedback
        {
            Comment = request.Comment,
            Rating = request.Rating,
            OrderId = orderId,
            ReviewerUserId = _doerlyRequestContext.UserId ?? -1
        };
        
        DbContext.OrderFeedbacks.Add(feedback);
        await DbContext.SaveChangesAsync();
        
        return HandlerResult.Success();
    }
}
