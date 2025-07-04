using System;
using System.Linq;
using System.Linq.Expressions;

namespace JffCsharpTools.Domain.Extensions
{
    /// <summary>
    /// Extension methods for LINQ operations to provide dynamic ordering and querying capabilities.
    /// These methods enable runtime property-based sorting and filtering of IQueryable collections.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Applies ordering to an IQueryable based on a property name and direction.
        /// This method allows for dynamic ordering of entities based on a specified property.
        /// If the property name is empty or null, the original query is returned without ordering.
        /// If the property does not exist, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TEntity">The entity type being queried</typeparam>
        /// <param name="query">The IQueryable to apply ordering to</param>
        /// <param name="orderDescending">True for descending order, false for ascending</param>
        /// <param name="orderByProperty">The property name to order by (supports nested properties with dot notation)</param>
        /// <returns>An ordered queryable with the specified sorting applied</returns>
        [Obsolete("Use OrderByProperty instead.")]
        public static IOrderedQueryable<TEntity> ApplyOrderBy<TEntity>(this IQueryable<TEntity> query, bool orderDescending, string orderByProperty)
        {
            var orderByDirection = orderDescending ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            Expression parent = parameter;
            if (!string.IsNullOrEmpty(orderByProperty))
            {
                var parts = orderByProperty.Split('.');

                foreach (var part in parts)
                {
                    parent = Expression.Property(parent, part);
                }
            }

            Expression conversion = Expression.Convert(parent, typeof(object));
            var orderByExp = Expression.Lambda<Func<TEntity, Object>>(conversion, parameter);

            var resultExpression = Expression.Call(
                typeof(Queryable),
                orderByDirection,
                new Type[] { type, orderByExp.ReturnType },
                query.Expression,
                Expression.Quote(orderByExp));

            return (IOrderedQueryable<TEntity>)query.Provider.CreateQuery<TEntity>(resultExpression);
        }

        /// <summary>
        /// Orders an IQueryable by a specified property name.
        /// This method allows for dynamic ordering of entities based on a specified property.
        /// If the property name is empty or null, the original query is returned without ordering.
        /// If the property does not exist, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The entity type being queried</typeparam>
        /// <param name="source">The IQueryable to apply ordering to</param>
        /// <param name="propertyName">The property name to order by</param>
        /// <param name="descending">True for descending order, false for ascending (default: false)</param>
        /// <returns>An ordered queryable with the specified sorting applied</returns>
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool descending = false)
        {
            if (string.IsNullOrEmpty(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(resultExpression);
        }

        /// <summary>
        /// Applies a secondary ordering to an IOrderedQueryable based on a property name and direction.
        /// This method allows for dynamic secondary ordering of entities based on a specified property.
        /// If the property name is empty or null, the original query is returned without secondary ordering.
        /// If the property does not exist, an exception will be thrown.
        /// This method is typically used after an initial OrderBy has been applied.
        /// </summary>
        /// <typeparam name="T">The entity type being queried</typeparam>
        /// <param name="source">The IOrderedQueryable to apply secondary ordering to</param>
        /// <param name="propertyName">The property name to use for secondary ordering</param>
        /// <param name="descending">True for descending order, false for ascending (default: false)</param>
        /// <returns>An ordered queryable with the additional sorting applied</returns>
        public static IQueryable<T> ThenByProperty<T>(this IOrderedQueryable<T> source, string propertyName, bool descending = false)
        {
            if (string.IsNullOrEmpty(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = descending ? "ThenByDescending" : "ThenBy";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(resultExpression);
        }

    }
}
