using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Devices.Shared.Extensions.ExpressionExtensions;

public static class ExpressionExtensions
{
    public static string GetMemberName(this Expression expression)
    {
        return expression switch
        {
            null => throw new ArgumentException("Invalid"),
            MemberExpression memberExpression => memberExpression.Member.Name,
            MethodCallExpression methodCallExpression => methodCallExpression.Method.Name,
            UnaryExpression unaryExpression => GetMemberName(unaryExpression),
            _ => throw new ArgumentException("Invalid")
        };
    }
    public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(this Expression<Func<TSource, bool>> root)
    {
        var visitor = new ParameterTypeVisitor<TSource, TTarget>();
        return (Expression<Func<TTarget, bool>>)visitor.Visit(root);
    }

    private class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
    {
        private ReadOnlyCollection<ParameterExpression>? _parameters;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameters?.FirstOrDefault(p => p.Name == node.Name)
                   ?? (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _parameters = VisitAndConvert(node.Parameters, "VisitLambda");
            return Expression.Lambda(Visit(node.Body), _parameters);
        }
    }

    private static string GetMemberName(UnaryExpression unaryExpression)
    {
        return unaryExpression.Operand is MethodCallExpression methodExpression
            ? methodExpression.Method.Name
            : ((MemberExpression)unaryExpression.Operand).Member.Name;
    }
}
