using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;
using Doerly.Extensions;
using System.Linq.Expressions;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Responses;
using OrderEntity = Doerly.Module.Order.DataAccess.Entities.Order;
using Doerly.Proxy.Profile;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrdersWithPaginationHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public GetOrdersWithPaginationHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy) : base(dbContext)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task<OperationResult<GetOrdersWithPaginationResponse>> HandleAsync(GetOrdersWithPaginationRequest dto)
    {
        var predicates = new List<Expression<Func<OrderEntity, bool>>>();

        if (dto.CustomerId.HasValue)
            predicates.Add(order => order.CustomerId == dto.CustomerId);
        if (dto.ExecutorId.HasValue)
            predicates.Add(order => order.ExecutorId == dto.ExecutorId);

        var (entities, totalCount) = await DbContext.Orders
            .AsNoTracking()
            .GetEntitiesWithPaginationAsync(dto.PageInfo, predicates);

        var profileIDs = entities.Select(x => x.CustomerId).Distinct().ToArray();

        var profiles = await _profileModuleProxy.GetProfilesAsync(profileIDs);

        var profileIDsDictionary = profiles.Value.ToDictionary(p => p.Id);

        var orders = entities.Select(order => new GetOrderResponse
        {
            Id = order.Id,
            CategoryId = order.CategoryId,
            CustomerId = order.CustomerId,
            Customer = profileIDsDictionary.TryGetValue(order.CustomerId, out var customerProfile)
                ? new ProfileInfo
                {
                    Id = customerProfile.Id,
                    FirstName = customerProfile.FirstName,
                    LastName = customerProfile.LastName,
                    AvatarUrl = customerProfile.ImageUrl
                }
                : null,
            BillId = order.BillId,
            Description = order.Description,
            DueDate = order.DueDate,
            Status = order.Status,
            ExecutionDate = order.ExecutionDate,
            ExecutorId = order.ExecutorId,
            Name = order.Name,
            PaymentKind = order.PaymentKind,
            Price = order.Price,
        }).ToList();

        var result = new GetOrdersWithPaginationResponse
        {
            Total = totalCount,
            Orders = orders
        };

        return OperationResult.Success(result);
    }
}
