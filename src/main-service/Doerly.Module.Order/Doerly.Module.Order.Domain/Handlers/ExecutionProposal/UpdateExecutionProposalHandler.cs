using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;
using Doerly.Domain;

namespace Doerly.Module.Order.Domain.Handlers;
public class UpdateExecutionProposalHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;
    public UpdateExecutionProposalHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult> HandleAsync(int id, UpdateExecutionProposalRequest dto)
    {
        var executionProposal = await DbContext.ExecutionProposals.Select(x => new { x.Id, x.SenderId })
            .FirstOrDefaultAsync(x => x.Id == id && x.SenderId == _doerlyRequestContext.UserId);
        if (executionProposal == null)
            return HandlerResult.Failure(Resources.Get("ExecutionProposalNotFound"));

        await DbContext.ExecutionProposals.Where(x => x.Id == id).ExecuteUpdateAsync(setters => setters
            .SetProperty(executionProposal => executionProposal.Comment, dto.Comment.Trim()));

        return HandlerResult.Success();
    }
}
