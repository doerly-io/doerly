using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Requests.Order;
using Doerly.Module.Order.Domain.Dtos.Responses.Order;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetOrderHistoryHandler : BaseOrderHandler
{
	public GetOrderHistoryHandler(OrderDbContext dbContext) : base(dbContext)
	{
	}
	public async Task<HandlerResult<List<GetOrderResponse>>> HandleAsync(GetOrderHistoryRequest dto)
	{
		var skip = dto.PageSize * (dto.PageNumber - 1);

		var orders = await DbContext.Orders
			.AsNoTracking()
			.Skip(skip)
			.Take(dto.PageSize)
			.Select(o => new GetOrderResponse {
			Id = o.Id,
			CategoryId = o.CategoryId,
			CustomerId = o.CustomerId,
			BillId = o.BillId,
			Description = o.Description,
			DueDate = o.DueDate,
			ExecutionDate = o.ExecutionDate,
			ExecutorId = o.ExecutorId,
			Name = o.Name,
			PaymentKind = o.PaymentKind,
			Price = o.Price,
		}).ToListAsync();

		return HandlerResult.Success(orders);
	}
}
