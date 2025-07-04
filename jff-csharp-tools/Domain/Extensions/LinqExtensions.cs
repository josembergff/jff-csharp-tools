using System;
using System.Linq;
using System.Linq.Expressions;

namespace JffCsharpTools.Domain.Extensions
{
    public static class LinqExtensions
    {
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
