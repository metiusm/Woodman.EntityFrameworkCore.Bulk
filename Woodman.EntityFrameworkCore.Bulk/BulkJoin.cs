﻿using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class BulkJoin
    {
        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<int> keys)
            where TEntity : class
        {
            return queryable.Join(keys.Select(k => k.ToString()));
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<long> keys)
            where TEntity : class
        {
            return queryable.Join(keys.Select(k => k.ToString()));
        }

        public static IQueryable<TEntity> Join<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<string> keys, char delimiter = ',')
            where TEntity : class
        {
            var toFind = keys?.ToList() ?? new List<string>();

            if (toFind == null || toFind.Count == 0)
            {
                return queryable.Where(e => false);
            }

            return queryable
                .BuildBulkExecutor()
                .Join(queryable, keys, delimiter);
        }
    }
}
