using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;
using Doerly.Domain;

namespace Doerly.Module.Order.Domain.Handlers;
public class ResolveExecutionProposalHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;
    public ResolveExecutionProposalHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id, ResolveExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.FirstOrDefaultAsync(x => x.Id == id && 
            (x.SenderId == _doerlyRequestContext.UserId || x.ReceiverId == _doerlyRequestContext.UserId));

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
