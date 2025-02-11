using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Enums;
using Doerly.Module.Order.DataAccess.Models;
using Doerly.Module.Order.Domain.Dtos.Requests.ExecutionProposal;
using Doerly.Module.Order.Domain.Dtos.Responses.ExecutionProposal;
using Doerly.Module.Order.Localization;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class SendExecutionProposalHandler : BaseOrderHandler
{
    public SendExecutionProposalHandler(OrderDbContext dbContext) : base(dbContext)
    {}

    public async Task<HandlerResult<SendExecutionProposalResponse>> HandleAsync(SendExecutionProposalRequest dto)
    {
        var order = await DbContext.Orders.FindAsync(dto.OrderId);
        if (order == null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("ORDER_NOT_FOUND"));

        var existingExecutionProposal = await DbContext.ExecutionProposals
            .FirstOrDefaultAsync(x => x.OrderId == dto.OrderId && x.ReceiverId == dto.ReceiverId);

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
