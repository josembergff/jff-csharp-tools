using System;
using System.Linq;
using System.Linq.Expressions;

namespace JffCsharpTools.Domain.Filters
{
    /// <summary>
    /// Static utility class for building and combining LINQ expression predicates.
    /// Provides methods to create base predicates and combine them using logical operations.
    /// This class is essential for building dynamic query filters at runtime.
    /// </summary>
    public static class PredicateBuilderFilter
    {
        /// <summary>
        /// Creates a predicate expression that always returns true.
        /// Used as a starting point for building complex filter expressions.
        /// </summary>
        /// <typeparam name="T">The entity type for the predicate</typeparam>
        /// <returns>An expression that evaluates to true for all entities</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Creates a predicate expression that always returns false.
        /// Used as a starting point when you want to exclude all entities initially.
        /// </summary>
        /// <typeparam name="T">The entity type for the predicate</typeparam>
        /// <returns>An expression that evaluates to false for all entities</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// Combines two predicate expressions using a logical OR operation.
        /// The resulting expression will return true if either of the input expressions evaluates to true.
        /// </summary>
        /// <typeparam name="T">The entity type for the predicates</typeparam>
        /// <param name="expr1">The first predicate expression</param>
        /// <param name="expr2">The second predicate expression to combine with OR logic</param>
        /// <returns>A combined expression representing expr1 OR expr2</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Combines two predicate expressions using a logical AND operation.
        /// The resulting expression will return true only if both input expressions evaluate to true.
        /// </summary>
        /// <typeparam name="T">The entity type for the predicates</typeparam>
        /// <param name="expr1">The first predicate expression</param>
        /// <param name="expr2">The second predicate expression to combine with AND logic</param>
        /// <returns>A combined expression representing expr1 AND expr2</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
