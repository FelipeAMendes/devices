using Devices.Shared.Entities;
using Devices.Shared.Extensions.ExpressionExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Devices.Shared.Extensions.ModelBuilderExtensions;

public static class ModelBuilderExtensions
{
    private static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

    public static void SetQueryFilter<TEntity, TEntityInterface>(this ModelBuilder builder,
        Expression<Func<TEntityInterface, bool>> filterExpression)
        where TEntityInterface : class
        where TEntity : class, TEntityInterface
    {
        var concreteExpression = filterExpression.Convert<TEntityInterface, TEntity>();

        builder.Entity<TEntity>().HasQueryFilter(concreteExpression);
    }

    public static void SetQueryFilterOnAllEntities<TEntityInterface>(this ModelBuilder builder,
        Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        var types = builder.Model.GetEntityTypes()
            .Where(t => t.BaseType == null)
            .Select(t => t.ClrType)
            .Where(t => typeof(TEntityInterface).IsAssignableFrom(t));

        foreach (var type in types)
            builder.SetEntityQueryFilter(type, filterExpression);
    }

    public static ModelBuilder ConfigureEntities(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var entitiesEF = assembly
            .GetTypes()
            .Where(t => t.IsClass
                        && !t.IsAbstract
                        && t.IsPublic
                        && typeof(IEntity).IsAssignableFrom(t));

        foreach (var entityEF in entitiesEF)
        {
            var nameSchema = entityEF.Namespace?.Split('.').Last();
            var entityTypeBuilder = modelBuilder.Entity(entityEF);

            var entity = modelBuilder
                .Entity(entityEF);

            entity
                .HasKey(nameof(IEntity.Id));

            entity
                .Property(nameof(IEntity.CreationTime))
                .IsRequired();

            entity
                .Property(nameof(IEntity.Removed))
                .IsRequired()
                .HasDefaultValue(false);

            entityTypeBuilder.ToTable(entityEF.Name, nameSchema);
        }

        return modelBuilder;
    }

    public static ModelBuilder ConfigureGlobalFilter(this ModelBuilder modelBuilder)
    {
        modelBuilder.SetQueryFilterOnAllEntities<IEntity>(x => !x.Removed);

        return modelBuilder;
    }

    private static void SetEntityQueryFilter<TEntityInterface>(this ModelBuilder builder, Type entityType,
        Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        SetQueryFilterMethod.MakeGenericMethod(entityType, typeof(TEntityInterface))
            .Invoke(null, [builder, filterExpression]);
    }
}