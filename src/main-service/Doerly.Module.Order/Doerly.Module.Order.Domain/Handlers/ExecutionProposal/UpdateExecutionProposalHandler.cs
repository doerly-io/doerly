using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;
using Doerly.Domain;
using Doerly.Module.Order.DataTransferObjects.Requests;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateExecutionProposalHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;
    public UpdateExecutionProposalHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<OperationResult> HandleAsync(int id, UpdateExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.Select(x => new { x.Id, x.SenderId })
            .FirstOrDefaultAsync(x => x.Id == id && x.SenderId == _doerlyRequestContext.UserId);
        if (executionProposal == null)
            return OperationResult.Failure(Resources.Get("ExecutionProposalNotFound"));

        await DbContext.ExecutionProposals.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(executionProposal => executionProposal.Comment, dto.Comment.Trim()));

        return OperationResult.Success();
    }
}
