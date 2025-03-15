using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Enums;
using Doerly.Module.Order.Domain.Dtos.Requests;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class ResolveExecutionProposalHandler : BaseOrderHandler
{
    public ResolveExecutionProposalHandler(OrderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<HandlerResult> HandleAsync(ResolveExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.FindAsync(dto.Id);

        if (executionProposal == null)
            return HandlerResult.Failure(Resources.Get("EXECUTION_PROPOSAL_NOT_FOUND"));

        executionProposal.Status = dto.ExecutionProposalStatus;
        if (executionProposal.Status == ExecutionProposalStatus.Accepted)
        {
            var order = await DbContext.Orders.FindAsync(executionProposal.OrderId);
            if (order == null)
                return HandlerResult.Failure(Resources.Get("ORDER_NOT_FOUND"));

            order.Status = OrderStatus.InProgress;
            if (order.CustomerId == executionProposal.ReceiverId)
                order.ExecutorId = executionProposal.SenderId;
            else
                order.ExecutorId = executionProposal.ReceiverId;

            await DbContext.ExecutionProposals
                .Where(x => x.OrderId == order.Id && x.Id != executionProposal.Id && x.Status == ExecutionProposalStatus.WaitingForApproval)
                .ForEachAsync(executionProposal =>
                {
                    executionProposal.Status = ExecutionProposalStatus.Revoked;
                });
        }

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
