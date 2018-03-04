using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class BulkJoin
    {
        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<int> keys)
            where TEntity : class
        {
            return queryable.Join(keys.Select(k => new object[] { k }));
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<long> keys)
            where TEntity : class
        {
            return queryable.Join(keys.Select(k => new object[] { k }));
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<string> keys)
            where TEntity : class
        {
            return queryable.Join(keys.Select(k => new object[] { k }));
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<object[]> keys)
            where TEntity : class
        {
            var toFind = keys?.ToList() ?? new List<object[]>();

            if (toFind == null || toFind.Count == 0)
            {
                return queryable.Where(e => false);
            }

            return queryable
                .BuildBulkExecutor()
                .Join(queryable, toFind);
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, params PropertyFilter<TEntity>[] propertySelectors)
            where TEntity : class
        {
            if (propertySelectors == null || propertySelectors.Length == 0)
            {
                return queryable;
            }

            return queryable
                .BuildBulkExecutor()
                .Join(queryable, propertySelectors);
        }
    }
}
