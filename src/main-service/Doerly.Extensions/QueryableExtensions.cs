using System.Linq.Expressions;

using Doerly.DataAccess.Models;
using Doerly.DataTransferObjects.Pagination;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Extensions;

public static class QueryableExtensions
{
    public static async Task<(IList<TEntity> Entities, int TotalCount)> GetEntitiesWithPaginationAsync<TEntity>(
        this IQueryable<TEntity> query,
        PageInfo pageInfo,
        IEnumerable<Expression<Func<TEntity, bool>>> predicates = null,
        Expression<Func<TEntity, TEntity>> selector = null,
        CancellationToken cancellationToken = default) where TEntity : BaseEntity
    {
        if (predicates != null && predicates.Any())
        {
            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (selector != null)
        {
            query = query.Select(selector);
        }

        var skip = pageInfo.Size * (pageInfo.Number - 1);
        var entities = await query.Skip(skip).Take(pageInfo.Size).ToListAsync(cancellationToken);

        return (entities, totalCount);
    }
}
