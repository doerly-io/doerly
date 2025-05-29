using Doerly.DataTransferObjects;
using Doerly.Domain;
using Doerly.Module.Payments.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Payments.Domain.Handlers;

public class SelectUserPaymentsHandler : BasePaymentHandler
{
    private readonly IDoerlyRequestContext _requestContext;

    public SelectUserPaymentsHandler(PaymentDbContext dbContext, IDoerlyRequestContext requestContext) : base(dbContext)
    {
        _requestContext = requestContext;
    }

    public async Task HandleAsync(CursorPaginationRequest cursorRequest)
    {
        var userId = _requestContext.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");
        
        var cursor = Cursor.Decode(cursorRequest.Cursor);

        var userPayments = await DbContext.Payments
            .AsNoTracking()
            .Where(p => p.Bill.PayerId == userId && p.Id > cursor.LastId)
            .OrderByDescending(x => x.LastModifiedDate)
            .Select(p => new
            {
                BillDescription = p.Bill.Description,
                BillId = p.BillId,
                Status = p.Status,
                Amount = p.Amount,
                Currency = p.Currency,
            })
            .Take(cursorRequest.PageSize + 1)
            .ToListAsync();
        
    }
}
