using System.Linq.Expressions;
using Doerly.DataTransferObjects.Pagination;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Responses;
using Doerly.Module.Order.Enums;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;

public class SelectOrdersWithFilterAndPaginationHandler : BaseOrderHandler
{
    private readonly IProfileModuleProxy _profileModuleProxy;

    public SelectOrdersWithFilterAndPaginationHandler(
        OrderDbContext dbContext,
        IProfileModuleProxy profileModuleProxy
    ) : base(dbContext)
    {
        _profileModuleProxy = profileModuleProxy;
    }

    public async Task<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>> HandleAsync(
        GetOrderWithFilterAndPaginationRequest dto)
    {
        var predicates = new List<Expression<Func<DataAccess.Entities.Order, bool>>>();

        predicates.Add(o => o.CategoryId == dto.CategoryId && o.Status == EOrderStatus.Placed);

        if (dto.RegionId.HasValue)
            predicates.Add(o => o.RegionId == dto.RegionId);
        if (dto.MinPrice.HasValue)
            predicates.Add(o => o.Price >= dto.MinPrice.Value);
        if (dto.MaxPrice.HasValue)
            predicates.Add(o => o.Price <= dto.MaxPrice.Value);
        if (!string.IsNullOrWhiteSpace(dto.SearchValue))
            predicates.Add(o => o.Name.Contains(dto.SearchValue) || o.Description.Contains(dto.SearchValue));

        var query = DbContext.Orders.AsNoTracking();
        query = predicates.Aggregate(query, (current, predicate) => current.Where(predicate));

        Expression<Func<DataAccess.Entities.Order, object>> orderKeySelector = dto.IsOrderByPrice
            ? o => o.Price
            : o => o.DueDate;

        query = dto.IsDescending ? query.OrderByDescending(orderKeySelector) : query.OrderBy(orderKeySelector);

        var filteredCount = await query.CountAsync();

        var orders = await query
            .Skip(dto.PageInfo.Number * dto.PageInfo.Size)
            .Take(dto.PageInfo.Size)
            .Select(order => new
            {
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                CategoryId = order.CategoryId,
                Name = order.Name,
                Description = order.Description,
                Price = order.Price,
                IsPriceNegotiable = order.IsPriceNegotiable,
                DueDate = order.DueDate,
                CreatedDate = order.DateCreated,
                UseProfileAddress = order.UseProfileAddress
            })
            .ToListAsync();

        var profiles = await _profileModuleProxy.GetProfilesShortInfoWithAvatarAsync(
            orders.Select(x => x.CustomerId));

        var ordersWithProfiles = orders.Select(order => new GetOrdersWithPaginationAndFilterResponse
        {
            OrderId = order.OrderId,
            CategoryId = order.CategoryId,
            Name = order.Name,
            Price = order.Price,
            DueDate = order.DueDate,
            Customer = MapProfileToProfileInfo(profiles, order.CustomerId),
            CreatedDate = order.CreatedDate
        }).ToList();

        var result = new BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>
        {
            Items = ordersWithProfiles,
            Count = filteredCount
        };

        return result;
    }

    private static ProfileInfo MapProfileToProfileInfo(IEnumerable<ProfileShortInfoWithAvatarDto> shortInfoWithAvatarDtos, int userId)
    {
        var profile = shortInfoWithAvatarDtos.First(p => p.UserId == userId);

        return new ProfileInfo
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            AvatarUrl = profile.AvatarUrl
        };
    }
}
