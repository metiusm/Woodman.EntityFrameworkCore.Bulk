﻿using System;
using System.Linq.Expressions;

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
                throw new Exception($"{updateExpressionName} must be of type {nameof(MemberInitExpression)}.");
            }

            return memberInitExpression;
        }
    }
}
