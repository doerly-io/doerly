using System.Linq.Expressions;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Models;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetExecutionProposalsWithPaginationHandler : BaseOrderHandler
{
    public GetExecutionProposalsWithPaginationHandler(OrderDbContext context) : base(context)
    { }

    public async Task<HandlerResult<GetExecutionProposalsWithPaginationResponse>> HandleAsync(GetExecutionProposalsWithPaginationRequest dto)
    {
        var predicates = new List<Expression<Func<ExecutionProposal, bool>>>();

        if (dto.ReceiverId.HasValue)
            predicates.Add(ep => ep.ReceiverId == dto.ReceiverId);
        if (dto.SenderId.HasValue)
            predicates.Add(ep => ep.SenderId == dto.SenderId);

        var (entities, totalCount) = await DbContext.ExecutionProposals
            .AsNoTracking()
            .GetEntitiesWithPaginationAsync(dto.PageInfo, predicates);

        var executionProposals = entities
            .Select(x => new GetExecutionProposalResponse
            {
                Id = x.Id,
                SenderId = x.SenderId,
                ReceiverId = x.ReceiverId,
                Status = x.Status
            }).ToList();

        var result = new GetExecutionProposalsWithPaginationResponse
        {
            Total = totalCount,
            ExecutionProposals = executionProposals
        };

        return HandlerResult.Success(result);
    }
}
