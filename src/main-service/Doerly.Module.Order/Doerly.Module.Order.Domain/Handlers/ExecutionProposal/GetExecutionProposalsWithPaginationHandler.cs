﻿using System.Linq.Expressions;

using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Doerly.Proxy.Profile;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Profile.DataTransferObjects.Profile;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetExecutionProposalsWithPaginationHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public GetExecutionProposalsWithPaginationHandler(OrderDbContext context, IProfileModuleProxy profileModuleProxy, IDoerlyRequestContext doerlyRequestContext) : base(context)
    {
        _profileModuleProxy = profileModuleProxy;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<OperationResult<GetExecutionProposalsWithPaginationResponse>> HandleAsync(GetExecutionProposalsWithPaginationRequest dto)
    {
        var predicates = new List<Expression<Func<ExecutionProposal, bool>>>();

        if (_doerlyRequestContext.UserId != null &&
            (_doerlyRequestContext.UserId == dto.ReceiverId || _doerlyRequestContext.UserId == dto.SenderId))
        {
            if (dto.ReceiverId.HasValue)
                predicates.Add(ep => ep.ReceiverId == dto.ReceiverId);
            else
                predicates.Add(ep => ep.SenderId == dto.SenderId);
        }
        else
        {
            throw new Exception(Resources.Get("InvalidAccess"));
        }

        List<OrderByDto<ExecutionProposal>> orderByDtos = [];
        orderByDtos.Add(new OrderByDto<ExecutionProposal>
        {
            Expression = proposal => proposal.DateCreated,
            IsDescending = true
        });
        orderByDtos.Add(new OrderByDto<ExecutionProposal>
        {
            Expression = proposal => proposal.Status,
        });

        var (entities, totalCount) = await DbContext.ExecutionProposals
            .AsNoTracking()
            .GetEntitiesWithPaginationAsync(dto.PageInfo, predicates, orderByDtos: orderByDtos);

        int[] profileIDs = [];

        if (dto.ReceiverId.HasValue)
            profileIDs = entities.Select(x => x.SenderId).Distinct().ToArray();
        else if (dto.SenderId.HasValue)
            profileIDs = entities.Select(x => x.ReceiverId).Distinct().ToArray();

        var profiles = await _profileModuleProxy.GetProfilesAsync(profileIDs);

        var profileIDsDictionary = profiles.Value.ToDictionary(p => p.Id);

        var executionProposals = entities
        .Select(executionProposal => new GetExecutionProposalResponse
        {
            Id = executionProposal.Id,
            OrderId = executionProposal.OrderId,
            SenderId = executionProposal.SenderId,
            Sender = profileIDsDictionary.TryGetValue(executionProposal.SenderId, out var senderProfile)
                ? new ProfileInfo
                {
                    Id = senderProfile.Id,
                    FirstName = senderProfile.FirstName,
                    LastName = senderProfile.LastName,
                    AvatarUrl = senderProfile.ImageUrl
                }
                : null,
            ReceiverId = executionProposal.ReceiverId,
            Receiver = profileIDsDictionary.TryGetValue(executionProposal.ReceiverId, out var receiverProfile)
                ? new ProfileInfo
                {
                    Id = receiverProfile.Id,
                    FirstName = receiverProfile.FirstName,
                    LastName = receiverProfile.LastName,
                    AvatarUrl = receiverProfile.ImageUrl
                }
                : null,
            Status = executionProposal.Status,
            DateCreated = executionProposal.DateCreated
        }).ToList();

        var result = new GetExecutionProposalsWithPaginationResponse
        {
            Total = totalCount,
            ExecutionProposals = executionProposals
        };

        return OperationResult.Success(result);
    }
}
