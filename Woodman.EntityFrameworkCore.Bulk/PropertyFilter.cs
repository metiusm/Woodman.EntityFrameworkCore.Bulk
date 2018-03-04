using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public class PropertyFilter<TEntity> where TEntity : class
    {
        public HashSet<object> Values { get; }

        public PropertyInfo Property { get; }

        public bool IsNullable { get; }

        public static PropertyFilter<TEntity> As<TProperty>(Expression<Func<TEntity, TProperty>> selector, IEnumerable<TProperty> values)
        {
            var propertyInfo = selector.EnsureMemberExpression().EnsureProperty();

            return new PropertyFilter<TEntity>(propertyInfo, values.Select(v => v == null ? null : (object)v));
        }

        private PropertyFilter(PropertyInfo propertyInfo, IEnumerable<object> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Values = new HashSet<object>(values.Distinct());

            Property = propertyInfo;

            IsNullable = Property.PropertyType.IsValueType
                ? Activator.CreateInstance(Property.PropertyType) == null
                    : true;

            foreach (var value in Values)
            {
                if (value == null && !IsNullable)
                {
                    throw new ArgumentException($"Invalid Property Value: Value cannot be null.");
                }
                else if (value.GetType() != Property.PropertyType)
                {
                    throw new ArgumentException($"Invalid Property Value: Expected typeof({Property.PropertyType.Name}) but received typeof({value.GetType().Name}).");
                }
            }
        }
    }
}
