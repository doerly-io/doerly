using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class AddOrderFeedbackHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public AddOrderFeedbackHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<OperationResult> HandleAsync(int orderId, OrderFeedbackRequest request)
    {
        var orderExists = await DbContext.Orders.AnyAsync(o => o.Id == orderId);
        if (!orderExists)
            return OperationResult.Failure("ORDER_NOT_FOUND");

        var feedback = new OrderFeedback
        {
            Comment = request.Comment,
            Rating = request.Rating,
            OrderId = orderId,
            ReviewerUserId = _doerlyRequestContext.UserId ?? -1
        };
        
        DbContext.OrderFeedbacks.Add(feedback);
        await DbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }
}
