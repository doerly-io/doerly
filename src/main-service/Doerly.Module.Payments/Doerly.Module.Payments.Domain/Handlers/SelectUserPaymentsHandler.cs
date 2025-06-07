using Doerly.DataTransferObjects;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Payments.Domain.Handlers;

public class SelectUserPaymentsHandler : BasePaymentHandler
{
    public SelectUserPaymentsHandler(PaymentDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<CursorPaginationResponse<PaymentHistoryItemResponse>> HandleAsync(int userId, CursorPaginationRequest cursorRequest)
    {
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

        return new CursorPaginationResponse<PaymentHistoryItemResponse>
        {
            Cursor = nextCursor,
            Items = userPayments
        };
    }
}
