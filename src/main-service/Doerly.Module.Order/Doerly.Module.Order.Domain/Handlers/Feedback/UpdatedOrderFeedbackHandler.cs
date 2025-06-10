using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class UpdatedOrderFeedbackHandler : BaseOrderHandler
{
    public UpdatedOrderFeedbackHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OperationResult> HandleAsync(int orderId, int feedbackId, OrderFeedbackRequest request)
    {
        await DbContext.OrderFeedbacks.Where(x => x.Id == feedbackId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Comment, request.Comment)
                .SetProperty(b => b.LastModifiedDate, DateTime.UtcNow)
                .SetProperty(y => y.Rating, request.Rating));
        
        return OperationResult.Success();
    }
}
