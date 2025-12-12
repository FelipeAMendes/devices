using Devices.Shared.Extensions.IQueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Devices.Shared.Extensions.IQueryableExtensions;

public static class QueryableExtensions
{
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
