using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    internal static class ExpressionExtensions
    {
        internal static MemberInitExpression EnsureMemberInitExpression<TEntity>(this Expression<Func<TEntity>> updateFactory) where TEntity : class
        {
            return updateFactory.Body.EnsureMemberInitExpression(nameof(updateFactory));
        }

        internal static MemberInitExpression EnsureMemberInitExpression<T, TEntity>(this Expression<Func<T, TEntity>> updateFactory) where TEntity : class
        {
            return updateFactory.Body.EnsureMemberInitExpression(nameof(updateFactory));
        }

        internal static MemberInitExpression EnsureMemberInitExpression(this Expression updateExpressionBody, string updateExpressionName)
        {
            while (updateExpressionBody.NodeType == ExpressionType.Convert || updateExpressionBody.NodeType == ExpressionType.ConvertChecked)
            {
                updateExpressionBody = ((UnaryExpression)updateExpressionBody).Operand;
            }
            var memberInitExpression = updateExpressionBody as MemberInitExpression;

            if (memberInitExpression == null)
            {
                throw new ArgumentException($"{updateExpressionName} must be of type {nameof(MemberInitExpression)}.");
            }

            return memberInitExpression;
        }

        internal static MemberExpression EnsureMemberExpression<TEntity, TProperty>(this Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        {
            if (navigationPropertyPath.Body is MemberExpression memberExpression)
            {
                return memberExpression;
            }
            else if(navigationPropertyPath.Body is UnaryExpression unaryExpression &&
                unaryExpression.NodeType == ExpressionType.Convert &&
                unaryExpression.Operand is MemberExpression operand)
            {
                return operand;
            }

            throw new ArgumentException($"{nameof(navigationPropertyPath)} must be of type {nameof(MemberExpression)}.");
        }

        internal static PropertyInfo EnsureProperty(this MemberExpression memberExpression)
        {
            if(memberExpression.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }

            throw new ArgumentException($"{nameof(memberExpression)} must be of type {nameof(PropertyInfo)}.");
        }
    }
}
