using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Responses.ExecutionProposal;
using Doerly.Module.Order.Localization;

namespace Doerly.Module.Order.Domain.Handlers.ExecutionProposal;
public class GetExecutionProposalByIdHandler : BaseOrderHandler
{
    public GetExecutionProposalByIdHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult<GetExecutionProposalResponse>> HandleAsync(int id)
    {
        var executionProposal = await DbContext.ExecutionProposals.FindAsync(id);

        if (executionProposal == null)
            return HandlerResult.Failure<GetExecutionProposalResponse>(Resources.Get("EXECUTION_PROPOSAL_NOT_FOUND"));

        return HandlerResult.Success(new GetExecutionProposalResponse
        {
            Id = executionProposal.Id,
            OrderId = executionProposal.OrderId,
            Comment = executionProposal.Comment,
            SenderId = executionProposal.SenderId,
            ReceiverId = executionProposal.ReceiverId,
            Status = executionProposal.Status
        });
    }
}
