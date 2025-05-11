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
        var order = await DbContext.Orders.Select(x => new { x.Id, x.CustomerId }).FirstOrDefaultAsync(x => x.Id == dto.OrderId);
        if (order == null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("ORDER_NOT_FOUND"));

        /* firstly check if sender is a customer and then check if the receiver already got a proposal 
         * (simply checking receiver won't work because if receiver is a customer then there might be several proposals for him),
         * but if sender is executor (not a customer) then check if the sender already sent a proposal to the receiver
         */
        var existingExecutionProposal = await DbContext.ExecutionProposals
            .FirstOrDefaultAsync(x => x.OrderId == dto.OrderId && 
                ((x.SenderId == order.CustomerId && x.ReceiverId == dto.ReceiverId) ||
                (x.SenderId != order.CustomerId && x.SenderId == dto.SenderId)));

        if (existingExecutionProposal != null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("EXECUTION_PROPOSAL_ALREADY_SENT"));

        var executionProposal = new ExecutionProposal()
        {
            OrderId = dto.OrderId,
            Comment = dto.Comment.Trim(),
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            Status = EExecutionProposalStatus.WaitingForApproval
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
