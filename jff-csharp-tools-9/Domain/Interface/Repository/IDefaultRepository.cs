using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Interface.Repository
{
    /// <summary>
    /// Generic repository interface that provides standard CRUD operations and data access patterns
    /// for entities that inherit from DefaultEntity. Implements common database operations
    /// including filtering, pagination, and batch operations.
    /// </summary>
    /// <typeparam name="T">The Entity Framework DbContext type</typeparam>
    public interface IDefaultRepository<T> where T : DbContext
    {
        /// <summary>
        /// Creates a new entity in the database
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entity">The entity instance to create</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>The created entity with updated properties (like generated ID)</returns>
        Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Creates multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entities">Collection of entities to create</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>Collection of created entities with updated properties</returns>
        Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves an entity by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key</typeparam>
        /// <param name="key">The primary key value</param>
        /// <param name="include">Array of navigation property names to include in the query</param>
        /// <returns>The entity with the specified key, or null if not found</returns>
        Task<TEntity> GetByKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves the first entity that matches the specified filter criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Expression defining the filter criteria</param>
        /// <param name="include">Array of navigation property names to include in the query</param>
        /// <returns>The first matching entity, or null if no match is found</returns>
        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves all entities that match the specified filter criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Expression defining the filter criteria</param>
        /// <param name="include">Array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for performance</param>
        /// <returns>Collection of entities matching the filter criteria</returns>
        Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a typed filter object that defines search criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter</typeparam>
        /// <param name="filter">The filter object containing search criteria</param>
        /// <param name="include">Array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for performance</param>
        /// <returns>Collection of entities matching the filter criteria</returns>
        Task<IEnumerable<TEntity>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves all entities that belong to a specific user
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="userId">The ID of the user who owns the entities</param>
        /// <param name="include">Array of navigation property names to include in the query</param>
        /// <returns>Collection of entities belonging to the specified user</returns>
        Task<IEnumerable<TEntity>> GetByUser<TEntity>(int userId, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set of entities that match the specified filter criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">Pagination parameters including page size and current page</param>
        /// <param name="filter">Expression defining the filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for performance</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set using a typed filter object
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter</typeparam>
        /// <param name="filter">The filter object containing search and pagination criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for performance</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set of entities that belong to a specific user
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">Pagination parameters including page size and current page</param>
        /// <param name="idUser">The ID of the user who owns the entities</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <param name="asNoTracking">Whether to disable change tracking for performance</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> pagination, int idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity identified by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key</typeparam>
        /// <param name="entity">The entity with updated values</param>
        /// <param name="key">The primary key of the entity to update</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityList">Collection of entities with updated values</param>
        /// <param name="forceDetach">Whether to force detach entities from the context after update</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>Task representing the asynchronous update operation</returns>
        Task UpdateBatch<TEntity>(IEnumerable<TEntity> entityList, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes entities that match the specified filter criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="filter">Expression defining which entities to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes multiple entities in a single batch operation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityList">Collection of entities to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>Task representing the asynchronous delete operation</returns>
        Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes an entity by its primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key</typeparam>
        /// <param name="key">The primary key of the entity to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        Task<bool> DeleteByKey<TEntity, TKey>(TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Saves all pending changes to the database
        /// </summary>
        /// <returns>Task representing the asynchronous save operation</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Rolls back all pending changes in the current transaction
        /// </summary>
        /// <returns>Task representing the asynchronous rollback operation</returns>
        Task Rollback();
    }
}