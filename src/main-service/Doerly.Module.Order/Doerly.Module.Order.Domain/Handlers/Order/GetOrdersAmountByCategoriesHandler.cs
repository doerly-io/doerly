using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataTransferObjects.Responses;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers.Order;
public class GetOrdersAmountByCategoriesHandler : BaseOrderHandler
{
    public GetOrdersAmountByCategoriesHandler(OrderDbContext dbContext) : base(dbContext)
    {}

    public async Task<List<GetOrdersAmountByCategoriesResponse>> HandleAsync()
    {
        var result = await DbContext.Orders.AsNoTracking()
            .Select(order => new
            {
                order.CategoryId,
            })
            .GroupBy(order => order.CategoryId)
            .OrderByDescending(g => g.Count())
            .Take(9)
            .Select(groupedOrders => new GetOrdersAmountByCategoriesResponse
            {
                CategoryId = groupedOrders.Key,
                Amount = groupedOrders.Count()
            }).ToListAsync();

        return result;
    }
}
