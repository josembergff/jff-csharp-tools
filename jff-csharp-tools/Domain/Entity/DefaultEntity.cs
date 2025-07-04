using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using JffCsharpTools.Domain.Filters;

namespace JffCsharpTools.Domain.Entity
{
    /// <summary>
    /// Base entity class that provides common properties and filtering functionality
    /// for all entities in the system. This class implements a self-referencing generic pattern
    /// to enable strongly-typed filtering and querying capabilities.
    /// </summary>
    /// <typeparam name="TEntity">The concrete entity type that inherits from this base class</typeparam>
    public class DefaultEntity<TEntity> where TEntity : DefaultEntity<TEntity>, new()
    {
        /// <summary>
        /// Unique identifier for the entity. Serves as the primary key in database storage.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the user who created this entity.
        /// Used for auditing and access control purposes.
        /// </summary>
        public int CreatorUserId { get; set; }

        /// <summary>
        /// Timestamp indicating when the entity was created.
        /// Automatically set during entity creation and never modified afterwards.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Optional timestamp indicating when the entity was last updated.
        /// Null for entities that have never been modified after creation.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Creates a default filter expression based on the current entity's property values.
        /// This method builds a dynamic LINQ expression that matches entities with the same
        /// Id, CreatorUserId, CreatedAt, and UpdatedAt values as this instance.
        /// </summary>
        /// <returns>A LINQ expression for filtering entities of type TEntity</returns>
        protected Expression<Func<TEntity, bool>> GetDefaultFilter()
        {
            // Collection to store individual filter predicates
            var filterList = new List<Expression<Func<TEntity, bool>>>();
            // Start with a base expression that always returns true
            Expression<Func<TEntity, bool>> filter = PredicateBuilderFilter.True<TEntity>();

            // Add Id filter if Id is greater than 0 (valid identifier)
            if (Id > 0)
                filterList.Add(x => x.Id == Id);

            // Add CreatorUserId filter if CreatorUserId is greater than 0 (valid user identifier)
            if (CreatorUserId > 0)
                filterList.Add(x => x.CreatorUserId == CreatorUserId);

            // Add CreatedAt filter if within valid date range
            if (CreatedAt > DateTime.MinValue && CreatedAt < DateTime.MaxValue)
                filterList.Add(x => x.CreatedAt.Date == CreatedAt.Date);

            // Add UpdatedAt filter if not null and within valid date range
            if (UpdatedAt != null && UpdatedAt > DateTime.MinValue && UpdatedAt < DateTime.MaxValue)
                filterList.Add(x => (x.UpdatedAt ?? DateTime.Now).Date == (UpdatedAt ?? DateTime.Now).Date);

            // Combine all filter predicates using logical AND operations
            foreach (Expression<Func<TEntity, bool>> predicado in filterList)
            {
                filter = predicado.And(filter);
            }

            return filter;
        }

        /// <summary>
        /// Virtual method that returns the filter expression for this entity.
        /// Base implementation returns the default filter. Override this method in derived classes
        /// to provide custom filtering logic while optionally including the base filter.
        /// </summary>
        /// <returns>A LINQ expression for filtering entities of type TEntity</returns>
        public virtual Expression<Func<TEntity, bool>> GetFilter()
        {
            return GetDefaultFilter();
        }
    }
}