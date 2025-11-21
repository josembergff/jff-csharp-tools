using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JffCsharpTools.Domain.Entity;

namespace JffCsharpTools.Domain.Filters
{
    /// <summary>
    /// Filter class for entities that inherit from DefaultEntity, providing date range filtering capabilities
    /// for creation and update timestamps. This class extends DefaultFilter to add entity-specific filtering.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
    public class DefaultEntityFilter<TEntity> : DefaultFilter<DefaultEntity<TEntity>> where TEntity : DefaultEntity<TEntity>, new()
    {
        /// <summary>
        /// Start date for filtering records by creation date (inclusive).
        /// Only records created on or after this date will be included.
        /// </summary>
        public DateTime? CreatedAtStart { get; set; }

        /// <summary>
        /// End date for filtering records by creation date (inclusive).
        /// Only records created on or before this date will be included.
        /// </summary>
        public DateTime? CreatedAtEnd { get; set; }

        /// <summary>
        /// Start date for filtering records by update date (inclusive).
        /// Only records updated on or after this date will be included.
        /// </summary>
        public DateTime? UpdatedAtStart { get; set; }

        /// <summary>
        /// End date for filtering records by update date (inclusive).
        /// Only records updated on or before this date will be included.
        /// </summary>
        public DateTime? UpdatedAtEnd { get; set; }

        /// <summary>
        /// Builds and returns a LINQ expression for filtering entities based on the specified date ranges.
        /// This method creates predicate expressions for CreatedAt and UpdatedAt date filtering and combines
        /// them using logical AND operations. Uses caching to avoid rebuilding the expression multiple times.
        /// </summary>
        /// <returns>A LINQ expression that can be used to filter DefaultEntity records</returns>
        public override Expression<Func<DefaultEntity<TEntity>, bool>> Where()
        {
            // Return cached expression if already built
            if (_where != null)
            {
                return _where;
            }

            // Collection to store individual filter predicates
            var whereList = new List<Expression<Func<DefaultEntity<TEntity>, bool>>>();

            // Add CreatedAt start date filter if specified and within valid range
            if (CreatedAtStart != null && CreatedAtStart > DateTime.MinValue && CreatedAtStart < DateTime.MaxValue)
                whereList.Add(x => x.CreatedAt.Date >= CreatedAtStart.Value.Date);

            // Add CreatedAt end date filter if specified and within valid range
            if (CreatedAtEnd != null && CreatedAtEnd > DateTime.MinValue && CreatedAtEnd < DateTime.MaxValue)
                whereList.Add(x => x.CreatedAt.Date <= CreatedAtEnd.Value.Date);

            // Add UpdatedAt start date filter if specified and within valid range
            // Uses DateTime.UtcNow as fallback for null UpdatedAt values
            if (UpdatedAtStart != null && UpdatedAtStart > DateTime.MinValue && UpdatedAtStart < DateTime.MaxValue)
                whereList.Add(x => (x.UpdatedAt ?? DateTime.UtcNow).Date >= (UpdatedAtStart ?? DateTime.UtcNow).Date);

            // Add UpdatedAt end date filter if specified and within valid range
            // Uses DateTime.UtcNow as fallback for null UpdatedAt values
            if (UpdatedAtEnd != null && UpdatedAtEnd > DateTime.MinValue && UpdatedAtEnd < DateTime.MaxValue)
                whereList.Add(x => (x.UpdatedAt ?? DateTime.UtcNow).Date <= (UpdatedAtEnd ?? DateTime.UtcNow).Date);

            // Start with a base expression that always returns true
            Expression<Func<DefaultEntity<TEntity>, bool>> where = PredicateBuilderFilter.True<DefaultEntity<TEntity>>();

            // Combine all filter predicates using logical AND operations
            foreach (Expression<Func<DefaultEntity<TEntity>, bool>> predicate in whereList)
            {
                where = PredicateBuilderFilter.And(where, predicate);
            }

            // Cache and return the combined expression
            return _where = where;
        }
    }
}