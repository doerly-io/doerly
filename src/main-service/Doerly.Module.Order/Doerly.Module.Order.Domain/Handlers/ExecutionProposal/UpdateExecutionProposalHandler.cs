using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Models;
using Doerly.Module.Order.Domain.Dtos.Requests;

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
            return HandlerResult.Failure(Resources.Get("EXECUTION_PROPOSAL_NOT_FOUND"));

        await DbContext.ExecutionProposals.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(executionProposal => executionProposal.Comment, dto.Comment)
            .SetProperty(executionProposal => executionProposal.SenderId, dto.SenderId)
            .SetProperty(executionProposal => executionProposal.ReceiverId, dto.ReceiverId));

        return HandlerResult.Success();
    }
}
