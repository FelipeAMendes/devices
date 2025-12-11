using Devices.Shared.Extensions.IQueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Devices.Shared.Extensions.IQueryableExtensions;

public static class QueryableExtensions
{
    public static IQueryable<T>? OrderBy<T>(this IQueryable<T> query, string orderByExpression)
    {
        if (string.IsNullOrEmpty(orderByExpression))
            return query;

        orderByExpression = orderByExpression.TrimStart().TrimEnd();

        var strs = orderByExpression.Split(' ');
        var propertyName = strs[0];
        string orderByMethod;

        if (strs.Length == 1)
            orderByMethod = "OrderBy";
        else
            orderByMethod = strs.Last().Equals("DESC", StringComparison.OrdinalIgnoreCase)
                ? "OrderByDescending"
                : "OrderBy";

        var pe = Expression.Parameter(query.ElementType);
        var me = Expression.Property(pe, propertyName);
        var orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] { query.ElementType, me.Type },
            query.Expression,
            Expression.Quote(Expression.Lambda(me, pe)));

        return query.Provider.CreateQuery(orderByCall) as IQueryable<T>;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

    public static IQueryable<TEntity> AsNoTracking<TEntity>([NotNull] this IQueryable<TEntity> source, bool enable)
        where TEntity : class
    {
        return enable ? source.AsNoTracking() : source;
    }

    public static IQueryable<TEntity> IgnoreQueryFilters<TEntity>([NotNull] this IQueryable<TEntity> source, bool enable) where TEntity : class
    {
        return enable
            ? source.IgnoreQueryFilters()
            : source;
    }
}
