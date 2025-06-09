using Doerly.Domain.Models;
using Doerly.Module.Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Doerly.Module.Catalog.DataAccess.Models.Service;
using CategoryEntity = Doerly.Module.Catalog.DataAccess.Models.Category;
using System.Linq.Expressions;
using Doerly.Extensions;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.Contracts.Responses;

namespace Doerly.Module.Catalog.Domain.Handlers.Service
{
    public class GetServicesWithPaginationHandler : BaseCatalogHandler
    {
        public GetServicesWithPaginationHandler(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult<GetServicesWithPaginationResponse>> HandleAsync(GetServiceWithPaginationRequest request)
        {
            var baseQuery = DbContext.Services
                .AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.FilterValues)
                .Where(s => !s.IsDeleted && s.IsEnabled);

            var predicates = new List<Expression<Func<ServiceEntity, bool>>>();

            if (request.CategoryId.HasValue)
            {
                var categoryIds = await GetAllDescendantCategoryIdsAsync(request.CategoryId.Value);
                predicates.Add(s => categoryIds.Contains(s.CategoryId.Value));
            }

            if (request.FilterValues is { Count: > 0 })
            {
                foreach (var (filterId, value) in request.FilterValues)
                {
                    var fid = filterId;
                    var val = value;
                    predicates.Add(s => s.FilterValues.Any(fv => fv.FilterId == fid && fv.Value == val));
                }
            }

            baseQuery = request.SortBy?.ToLower() switch
            {
                "name_asc" => baseQuery.OrderBy(s => s.Name),
                "name_desc" => baseQuery.OrderByDescending(s => s.Name),
                _ => baseQuery.OrderBy(s => s.Name)
            };

            var (entities, totalCount) = await baseQuery.GetEntitiesWithPaginationAsync(
                request.PageInfo,
                predicates
            );

            var dtos = entities.Select(s => new GetServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CategoryId = s.CategoryId,
                CategoryName = s.Category?.Name,
                UserId = s.UserId,
                Price = s.Price,
                IsEnabled = s.IsEnabled,
                IsDeleted = s.IsDeleted,
                CategoryPath = GetCategoryPath(s.Category)
            }).ToList();

            var response = new GetServicesWithPaginationResponse
            {
                Total = totalCount,
                Orders = dtos
            };

            return OperationResult.Success(response);
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
                var children = allCategories.Where(c => c.ParentId == parentId).ToList();
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
