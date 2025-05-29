using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetExecutionProposalByIdHandler : BaseOrderHandler
{
    public GetExecutionProposalByIdHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult<GetExecutionProposalResponse>> HandleAsync(int id)
    {
        var executionProposal = await DbContext.ExecutionProposals.FindAsync(id);

        if (executionProposal == null)
            return HandlerResult.Failure<GetExecutionProposalResponse>(Resources.Get("ExecutionProposalNotFound"));

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
