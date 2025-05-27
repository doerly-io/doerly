using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Microsoft.EntityFrameworkCore;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Extensions;
using System.Linq.Expressions;

using OrderEntity = Doerly.Module.Order.DataAccess.Models.Order;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrdersWithPaginationHandler : BaseOrderHandler
{
    public GetOrdersWithPaginationHandler(OrderDbContext dbContext) : base(dbContext)
    { }

    public async Task<HandlerResult<GetOrdersWithPaginationResponse>> HandleAsync(GetOrdersWithPaginationRequest dto)
    {
        var predicates = new List<Expression<Func<OrderEntity, bool>>>();

        if (dto.CustomerId.HasValue)
            predicates.Add(order => order.CustomerId == dto.CustomerId);
        if (dto.ExecutorId.HasValue)
            predicates.Add(order => order.ExecutorId == dto.ExecutorId);

        var (entities, totalCount) = await DbContext.Orders
            .AsNoTracking()
            .GetEntitiesWithPaginationAsync(dto.PageInfo, predicates);

        var orders = entities
            .Select(o => new GetOrderResponse
            {
                Id = o.Id,
                CategoryId = o.CategoryId,
                CustomerId = o.CustomerId,
                BillId = o.BillId,
                Description = o.Description,
                DueDate = o.DueDate,
                Status = o.Status,
                ExecutionDate = o.ExecutionDate,
                ExecutorId = o.ExecutorId,
                Name = o.Name,
                PaymentKind = o.PaymentKind,
                Price = o.Price,
            }).ToList();

        var result = new GetOrdersWithPaginationResponse
        {
            Total = totalCount,
            Orders = orders
        };

        return HandlerResult.Success(result);
    }
}
