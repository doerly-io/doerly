using Doerly.Module.Catalog.DataAccess;
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

        List<int> categoryIds = [];
        if (response.Count != 0)
        {
            categoryIds = response.Select(r => r.CategoryId).ToList() ?? [];
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

        if (response.Count < 10)
        {
            var otherCategories = await DbContext.Categories.AsNoTracking()
                .Where(c => !categoryIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Name })
                .Take(10 - categoryIds.Count)
                .ToListAsync();

            foreach (var category in otherCategories)
            {
                response.Add(new GetOrdersAmountByCategoriesResponse
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    Amount = 0
                });
            }
        }

        return response;
    }
}