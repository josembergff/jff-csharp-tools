using System;
using System.Linq.Expressions;
using System.Reflection;

namespace JffCsharpTools.Domain.Filters
{
    /// <summary>
    /// Base filter class that provides pagination, ordering, and basic filtering functionality
    /// for any entity type. This class serves as the foundation for more specific filter implementations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be filtered</typeparam>
    public class DefaultFilter<TEntity>
    {
        private int _pageSize;
        private int _pageNumber;
        protected string _orderBy;
        protected bool _asc;

        /// <summary>
        /// Maximum number of results allowed per page to prevent performance issues
        /// </summary>
        public const int MAX_RESULT_SIZE = 10;

        /// <summary>
        /// Cached WHERE expression to avoid rebuilding the same filter multiple times
        /// </summary>
        protected Expression<Func<TEntity, bool>> _where;

        /// <summary>
        /// When set to true, pagination will be bypassed and all results will be returned
        /// </summary>
        public bool IgnorePagination = false;

        /// <summary>
        /// Initializes a new instance of DefaultFilter with default pagination settings.
        /// Sets page size to MAX_RESULT_SIZE and page number to 1.
        /// </summary>
        public DefaultFilter()
        {
            _pageSize = MAX_RESULT_SIZE;
            _pageNumber = 1;
        }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// Automatically validates the value - sets to 10 if value is less than 1.
        /// </summary>
        public int Count
        {
            get { return _pageSize; }
            set
            {
                if (value >= 1)
                    _pageSize = value;
                else
                    _pageSize = 10;
            }
        }

        /// <summary>
        /// Gets or sets the current page number (1-based indexing).
        /// Automatically validates the value - sets to 1 if value is less than 1.
        /// </summary>
        public int Page
        {
            get { return _pageNumber; }
            set
            {
                if (value >= 1)
                    _pageNumber = value;
                else
                    _pageNumber = 1;
            }
        }

        /// <summary>
        /// Calculates the number of items to skip for pagination.
        /// Returns 0 if IgnorePagination is true, otherwise calculates (Page - 1) * Count.
        /// </summary>
        public int SkipTotal
        {
            get { return IgnorePagination ? 0 : (Page - 1) * Count; }
        }

        /// <summary>
        /// Gets or sets the property name to order results by.
        /// Should match a valid property name of the entity type.
        /// </summary>
        public string OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        /// <summary>
        /// Gets or sets the sort direction.
        /// True for ascending order, false for descending order.
        /// </summary>
        public bool Asc
        {
            get { return _asc; }
            set { _asc = value; }
        }

        /// <summary>
        /// Virtual method that returns the WHERE clause expression for filtering entities.
        /// Base implementation returns a predicate that always evaluates to true (no filtering).
        /// Override this method in derived classes to implement specific filtering logic.
        /// </summary>
        /// <returns>A LINQ expression representing the filter criteria</returns>
        public virtual Expression<Func<TEntity, bool>> Where() { return c => true; }

        /// <summary>
        /// Validates that the OrderBy property contains a valid property name for the current entity type.
        /// Uses reflection to check if the specified property name exists on the filter class.
        /// Note: This method appears to have a logical error in the implementation.
        /// </summary>
        public virtual void CheckPropertieNameOrderBy()
        {
            Type typeObject = GetType();
            PropertyInfo[] properties = typeObject.GetProperties();
            bool ret = false;
            foreach (var item in properties)
                ret = !ret && item.Name == _orderBy ? true : ret;
        }
    }
}