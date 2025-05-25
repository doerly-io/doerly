using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateExecutionProposalHandler : BaseOrderHandler
{
    public UpdateExecutionProposalHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult> HandleAsync(int id, UpdateExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);
        if (executionProposal == 0)
            return HandlerResult.Failure(Resources.Get("ExecutionProposalNotFound"));

        await DbContext.ExecutionProposals.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(executionProposal => executionProposal.Comment, dto.Comment)
            /*.SetProperty(executionProposal => executionProposal.SenderId, dto.SenderId)
            .SetProperty(executionProposal => executionProposal.ReceiverId, dto.ReceiverId)*/);

        return HandlerResult.Success();
    }
}
