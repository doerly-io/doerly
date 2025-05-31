using Doerly.DataTransferObjects;
using Doerly.Domain;
using Doerly.Module.Payments.Contracts;
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

    public async Task<CursorPaginationResponse<object>> HandleAsync(CursorPaginationRequest cursorRequest)
    {
        var userId = _requestContext.UserId;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var userPaymentsQuery = DbContext.Payments
            .AsNoTracking()
            .Where(p => p.Bill.PayerId == userId);

        var cursor = Cursor.Decode(cursorRequest.Cursor);
        if (cursor.LastId != null)
            userPaymentsQuery = userPaymentsQuery.Where(p => p.Id > cursor.LastId);

        var userPayments = await userPaymentsQuery
            .OrderByDescending(x => x.LastModifiedDate)
            .Select(p => new PaymentHistoryItemResponse
            {
                PaymentId = p.Id,
                Description = p.Description,
                BillId = p.BillId,
                Status = p.Status,
                Amount = p.Amount,
                Currency = p.Currency,
                Action = p.Action,
                CreatedAt = p.DateCreated
            })
            .Take(cursorRequest.PageSize + 1)
            .ToListAsync();

        var hasMode = userPayments.Count > cursorRequest.PageSize;
        string? nextCursor = null;
        if (hasMode)
        {
            var lastId = userPayments.Last().PaymentId;
            nextCursor = Cursor.Encode(lastId);
            userPayments.RemoveAt(cursorRequest.PageSize);
        }

        return new CursorPaginationResponse<object>()
        {
            Cursor = nextCursor,
            Items = userPayments
        };
    }
}
