using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Requests;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateExecutionProposalHandler : BaseOrderHandler
{
    public UpdateExecutionProposalHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult> HandleAsync(UpdateExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.FindAsync(dto.Id);
        if (executionProposal == null)
            return HandlerResult.Failure(Resources.Get("EXECUTION_PROPOSAL_NOT_FOUND"));

        executionProposal.Comment = dto.Comment;
        executionProposal.SenderId = dto.SenderId;
        executionProposal.ReceiverId = dto.ReceiverId;

        await DbContext.SaveChangesAsync();
        return HandlerResult.Success();
    }
}
