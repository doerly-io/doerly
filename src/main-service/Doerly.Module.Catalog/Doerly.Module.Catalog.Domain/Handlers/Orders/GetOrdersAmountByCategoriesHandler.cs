using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Handlers;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Proxy.Orders;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Catalog.Domain.Handlers.Orders;
public class GetOrdersAmountByCategoriesHandler : BaseCatalogHandler
{
    private readonly IOrdersModuleProxy _ordersModuleProxy;

    public GetOrdersAmountByCategoriesHandler(CatalogDbContext catalogDbContext, IOrdersModuleProxy ordersModuleProxy)
        : base(catalogDbContext)
    {
        _ordersModuleProxy = ordersModuleProxy;
    }

    public async Task<List<GetOrdersAmountByCategoriesResponse>> HandleAsync()
    {
        var response = await _ordersModuleProxy.GetOrdersAmountByCategoriesAsync();
        if (response.Count != 0)
        {
            var categoryIds = response.Select(r => r.CategoryId).ToList();
            var categories = await DbContext.Categories.AsNoTracking()
                .Select(c => new { c.Id, c.Name })
                .Where(c => categoryIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Name);

            foreach (var item in response)
            {
                if (categories.TryGetValue(item.CategoryId, out var categoryName))
                {
                    item.CategoryName = categoryName;
                }
            }
        }
        return response;
    }
}
