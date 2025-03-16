using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Enums;
using Doerly.Module.Order.DataAccess.Models;
using Doerly.Module.Order.Domain.Dtos.Requests;
using Doerly.Module.Order.Domain.Dtos.Responses;

using Microsoft.EntityFrameworkCore;
using Doerly.Localization;

namespace Doerly.Module.Order.Domain.Handlers;
public class SendExecutionProposalHandler : BaseOrderHandler
{
    public SendExecutionProposalHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult<SendExecutionProposalResponse>> HandleAsync(SendExecutionProposalRequest dto)
    {
        var order = await DbContext.Orders.FindAsync(dto.OrderId);
        if (order == null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("ORDER_NOT_FOUND"));

        var existingExecutionProposal = await DbContext.ExecutionProposals
            .FirstOrDefaultAsync(x => x.OrderId == dto.OrderId && 
                ((x.SenderId == order.CustomerId && x.ReceiverId == dto.ReceiverId) ||
                (x.SenderId != order.CustomerId && x.SenderId == dto.SenderId)));

        if (existingExecutionProposal != null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("EXECUTION_PROPOSAL_ALREADY_SENT"));

        var executionProposal = new ExecutionProposal()
        {
            OrderId = dto.OrderId,
            Comment = dto.Comment,
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            Status = ExecutionProposalStatus.WaitingForApproval
        };

        DbContext.ExecutionProposals.Add(executionProposal);
        await DbContext.SaveChangesAsync();

        var result = new SendExecutionProposalResponse()
        {
            Id = executionProposal.Id
        };

        return HandlerResult.Success(result);
    }
}
