using System;
using System.ComponentModel.DataAnnotations;

namespace JffCsharpTools.Domain.Entity
{
    /// <summary>
    /// Base entity class that provides common properties and filtering functionality
    /// for all entities in the system. This class uses a Guid to identify the creator user,
    /// implementing a self-referencing generic pattern to enable strongly-typed filtering and querying capabilities.
    /// </summary>
    /// <typeparam name="TEntity">The concrete entity type that inherits from this base class</typeparam>
    public class DefaultBasicGuidEntity
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
        public Guid CreatorUserId { get; set; }

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
    }
}