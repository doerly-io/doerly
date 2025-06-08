using Doerly.Domain.Models;
using Doerly.Module.Order.Contracts.Dtos.Requests;
using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class UpdatedOrderFeedbackHandler : BaseOrderHandler
{
    public UpdatedOrderFeedbackHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<HandlerResult> HandleAsync(int orderId, int feedbackId, OrderFeedbackRequest request)
    {
        await DbContext.OrderFeedbacks.Where(x => x.Id == feedbackId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Comment, request.Comment)
                .SetProperty(y => y.Rating, request.Rating));
        
        return HandlerResult.Success();
    }
}
