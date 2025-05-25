using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.Contracts.Dtos;

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
            return HandlerResult.Failure(Resources.Get("ExecutionProposalNotFound"));

        executionProposal.Status = dto.Status;
        if (executionProposal.Status == EExecutionProposalStatus.Accepted)
        {
            var order = await DbContext.Orders.FindAsync(executionProposal.OrderId);
            if (order == null)
                return HandlerResult.Failure(Resources.Get("OrderNotFound"));

            order.Status = EOrderStatus.InProgress;
            if (order.CustomerId == executionProposal.ReceiverId)
                order.ExecutorId = executionProposal.SenderId;
            else
                order.ExecutorId = executionProposal.ReceiverId;

            await DbContext.ExecutionProposals
                .Where(x => x.OrderId == order.Id && x.Id != executionProposal.Id && x.Status == EExecutionProposalStatus.Pending)
                .ForEachAsync(executionProposal =>
                {
                    executionProposal.Status = EExecutionProposalStatus.Revoked;
                });
        }

        await DbContext.SaveChangesAsync();

        return HandlerResult.Success();
    }
}
