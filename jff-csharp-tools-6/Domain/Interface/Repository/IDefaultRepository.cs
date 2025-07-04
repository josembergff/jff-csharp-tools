using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Domain.Interface.Repository
{
    /// <summary>
    /// Default repository interface providing data access operations for entities
    /// This interface defines the contract for repositories that handle direct database operations
    /// </summary>
    /// <typeparam name="T">The DbContext type that will be used for database operations</typeparam>
    public interface IDefaultRepository<T> where T : DbContext
    {
        /// <summary>
        /// Creates a new entity in the database
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entity">The entity instance to be created</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>The created entity with populated ID and audit fields</returns>
        Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Creates multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entities">The collection of entities to be created</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>The collection of created entities with populated IDs and audit fields</returns>
        Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a single entity by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <param name="include">Optional array of navigation property names to include in the query</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<TEntity> GetByKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves the first entity that matches the specified filter or default if none found
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Lambda expression to filter the entities</param>
        /// <param name="include">Optional array of navigation property names to include in the query</param>
        /// <returns>The first matching entity or null if none found</returns>
        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves all entities that match the specified filter
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Lambda expression to filter the entities</param>
        /// <param name="include">Optional array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better performance (default: false)</param>
        /// <returns>Collection of entities matching the filter</returns>
        Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a custom filter object that extends DefaultFilter
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter for the entity</typeparam>
        /// <param name="filter">The filter object containing search criteria and parameters</param>
        /// <param name="include">Optional array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better performance (default: false)</param>
        /// <returns>Collection of entities matching the filter criteria</returns>
        Task<IEnumerable<TEntity>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves entities filtered by user ID (usually CreatorUserId)
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="userId">The user ID to filter entities by</param>
        /// <param name="include">Optional array of navigation property names to include in the query</param>
        /// <returns>Collection of entities belonging to the specified user</returns>
        Task<IEnumerable<TEntity>> GetByUser<TEntity>(int userId, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support and custom filtering
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">The pagination model containing page size, page number, and ordering information</param>
        /// <param name="filter">Lambda expression to filter the entities</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better performance (default: false)</param>
        /// <returns>Paginated results with metadata including total count and page information</returns>
        Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination using a custom filter object
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter and includes pagination</typeparam>
        /// <param name="filter">The filter object containing search criteria and pagination parameters</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better performance (default: false)</param>
        /// <returns>Paginated and filtered results with metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination filtered by user ID
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">The pagination model containing page size, page number, and ordering information</param>
        /// <param name="idUser">The user ID to filter entities by (usually CreatorUserId)</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better performance (default: false)</param>
        /// <returns>Paginated user-specific results with metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> pagination, int idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="entity">The entity instance with updated values</param>
        /// <param name="key">The primary key value of the entity to update</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityList">The collection of entities to be updated</param>
        /// <param name="forceDetach">Whether to detach entities after update to prevent tracking issues (default: false)</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateBatch<TEntity>(IEnumerable<TEntity> entityList, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes entities that match the specified filter
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Lambda expression to identify entities to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if any entities were deleted, false otherwise</returns>
        Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityList">The collection of entities to be deleted</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes a single entity by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="key">The primary key value of the entity to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if the entity was deleted, false if not found</returns>
        Task<bool> DeleteByKey<TEntity, TKey>(TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Saves all pending changes to the database
        /// </summary>
        /// <returns>Task representing the asynchronous save operation</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Rolls back all pending changes, discarding any unsaved modifications
        /// </summary>
        /// <returns>Task representing the asynchronous rollback operation</returns>
        Task Rollback();
    }
}