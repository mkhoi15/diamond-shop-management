using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Common;

public static class QueryableExtension
{
    public static IQueryable<TEntity> IncludeProperties<TEntity>(this IQueryable<TEntity> query, 
        Expression<Func<TEntity, object?>>[] includesProperties)
        where TEntity : class
    {
        if (includesProperties.Length == 0) return query;

        return includesProperties.Aggregate(query, (current, includeProperty) 
            => current.Include(includeProperty));
    }
    
    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> query, 
        bool condition,
        Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        return condition ? query.Where(predicate) : query;
    }
}