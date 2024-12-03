using System;
using System.Linq;
using System.Linq.Expressions;

namespace JffCsharpTools.Domain.Filters
{
    public static class PredicateConstructorFilter
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                           Expression<Func<T, bool>> expr2)
        {
            var exprInvocada = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, exprInvocada), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                           Expression<Func<T, bool>> expr2)
        {
            var exprInvocada = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, exprInvocada), expr1.Parameters);
        }
    }
}