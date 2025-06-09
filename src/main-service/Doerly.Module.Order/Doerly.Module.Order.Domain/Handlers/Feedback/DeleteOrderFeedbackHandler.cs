using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class DeleteOrderFeedbackHandler : BaseOrderHandler
{
    public DeleteOrderFeedbackHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task HandleAsync(int orderId, int feedbackId)
    {
        await DbContext.OrderFeedbacks.Where(x => x.Id == feedbackId && x.OrderId == orderId)
            .ExecuteDeleteAsync();
    }
}
