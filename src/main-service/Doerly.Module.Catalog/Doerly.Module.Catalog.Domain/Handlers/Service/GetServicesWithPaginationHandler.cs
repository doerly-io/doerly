using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.Contracts.Responses;
using Doerly.Module.Catalog.DataAccess;
using Doerly.Module.Catalog.DataAccess.Models;
using Doerly.Proxy.Profile;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServicesWithPaginationHandler : BaseCatalogHandler
    {
        private const string NameAsc = "name_asc";
        private const string NameDesc = "name_desc";
        private const string PriceAsc = "price_asc";
        private const string PriceDesc = "price_desc";

        private readonly IProfileModuleProxy _profileModuleProxy;

        public GetServicesWithPaginationHandler(
            CatalogDbContext dbContext,
            IProfileModuleProxy profileModuleProxy) : base(dbContext)
        {
            _profileModuleProxy = profileModuleProxy;
        }

        public async Task<OperationResult<GetServicesWithPaginationResponse>> HandleAsync(GetServiceWithPaginationRequest request)
        {
            var baseQuery = DbContext.Services
                .AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.FilterValues)
                    .ThenInclude(fv => fv.Filter)
                .Where(s => !s.IsDeleted && s.IsEnabled);

            var predicates = new List<Expression<Func<ServiceEntity, bool>>>();

            if (request.CategoryId.HasValue)
            {
                var categoryIds = await GetAllDescendantCategoryIdsAsync(request.CategoryId.Value);
                predicates.Add(s => categoryIds.Contains(s.CategoryId ?? 0));
            }

            if (request.FilterValues?.Count > 0)
            {
                var serviceParam = Expression.Parameter(typeof(ServiceEntity), "s");
                var filterValueParam = Expression.Parameter(typeof(ServiceFilterValue), "fv");

                Expression combinedPredicate = null;

                foreach (var filterValue in request.FilterValues)
                {
                    var filterIdCheck = Expression.Equal(
                        Expression.Property(filterValueParam, nameof(ServiceFilterValue.FilterId)),
                        Expression.Constant(filterValue.FilterId));

                    var valueCheck = Expression.Equal(
                        Expression.Property(filterValueParam, nameof(ServiceFilterValue.Value)),
                        Expression.Constant(filterValue.Value));

                    var predicateForThisFilter = Expression.AndAlso(filterIdCheck, valueCheck);

                    combinedPredicate = combinedPredicate == null
                        ? predicateForThisFilter
                        : Expression.OrElse(combinedPredicate, predicateForThisFilter);
                }

                var anyMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == nameof(Enumerable.Any)
                             && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(ServiceFilterValue));

                var anyCallExpression = Expression.Call(
                    anyMethod,
                    Expression.Property(serviceParam, nameof(ServiceEntity.FilterValues)),
                    Expression.Lambda<Func<ServiceFilterValue, bool>>(combinedPredicate!, filterValueParam));

                var lambda = Expression.Lambda<Func<ServiceEntity, bool>>(anyCallExpression, serviceParam);

                baseQuery = baseQuery.Where(lambda);
            }


            foreach (var predicate in predicates)
                baseQuery = baseQuery.Where(predicate);

            baseQuery = request.SortBy?.ToLower() switch
            {
                NameAsc => baseQuery.OrderBy(s => s.Name),
                NameDesc => baseQuery.OrderByDescending(s => s.Name),
                PriceAsc => baseQuery.OrderBy(s => s.Price),
                PriceDesc => baseQuery.OrderByDescending(s => s.Price),
                _ => baseQuery.OrderBy(s => s.Name)
            };

            if (!string.IsNullOrWhiteSpace(request.SearchValue))
            {
                var lowerSearch = request.SearchValue.ToLowerInvariant();
                baseQuery = baseQuery.Where(s => s.Name != null && s.Name.ToLower().Contains(lowerSearch));
            }

            var (entities, totalCount) = await baseQuery.GetEntitiesWithPaginationAsync(request.PageInfo);

            var dtos = new List<GetServiceResponse>();

            foreach (var service in entities)
            {
                var userProfile = await _profileModuleProxy.GetProfileAsync(service.UserId);

                dtos.Add(new GetServiceResponse
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    CategoryId = service.CategoryId,
                    CategoryName = service.Category?.Name,
                    UserId = service.UserId,
                    Price = service.Price,
                    IsEnabled = service.IsEnabled,
                    IsDeleted = service.IsDeleted,
                    CategoryPath = GetCategoryPath(service.Category),
                    User = userProfile.Value,
                    FilterValues = service.FilterValues.Select(fv => new FilterValueResponse
                    {
                        FilterId = fv.FilterId,
                        FilterName = fv.Filter.Name,
                        FilterType = fv.Filter.Type,
                        Value = fv.Value
                    }).ToList()
                });
            }

            return OperationResult.Success(new GetServicesWithPaginationResponse
            {
                Total = dtos.Count,
                Services = dtos
            });
        }


        private List<string> GetCategoryPath(CategoryEntity? category)
        {
            var path = new List<string>();
            while (category != null)
            {
                path.Insert(0, category.Name);
                category = category.Parent;
            }
            return path;
        }

        private async Task<List<int>> GetAllDescendantCategoryIdsAsync(int rootCategoryId)
        {
            var allCategories = await DbContext.Categories
                .AsNoTracking()
                .Select(c => new { c.Id, c.ParentId })
                .ToListAsync();

            var result = new List<int> { rootCategoryId };

            void CollectChildren(int parentId)
            {
                var children = allCategories.Where(c => c.ParentId == parentId);
                foreach (var child in children)
                {
                    result.Add(child.Id);
                    CollectChildren(child.Id);
                }
            }

            CollectChildren(rootCategoryId);
            return result;
        }
    }
}
