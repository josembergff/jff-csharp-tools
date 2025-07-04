using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Domain.Interface.Repository
{
    /// <summary>
    /// Generic repository interface that provides standard data access operations for entities.
    /// This interface defines a comprehensive set of CRUD operations, filtering, pagination, and batch processing capabilities.
    /// All operations are asynchronous and work with entities that inherit from DefaultEntity.
    /// The repository pattern abstracts the data access layer and provides a consistent interface for data operations.
    /// </summary>
    /// <typeparam name="T">The DbContext type used for database operations</typeparam>
    public interface IDefaultRepository<T> where T : DbContext
    {
        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to create</typeparam>
        /// <param name="entity">The entity instance to create in the database</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>The created entity with any generated values (like ID) populated</returns>
        Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Creates multiple entities in a single batch operation for improved performance.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to create</typeparam>
        /// <param name="entities">The collection of entities to create in the database</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>The collection of created entities with any generated values populated</returns>
        Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a single entity by its primary key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <param name="include">Array of navigation property names to include in the query (eager loading)</param>
        /// <returns>The entity with the specified key, or null if not found</returns>
        Task<TEntity> GetByKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves the first entity that matches the specified filter criteria, or null if no match is found.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="filter">Lambda expression defining the filter criteria</param>
        /// <param name="include">Array of navigation property names to include in the query (eager loading)</param>
        /// <returns>The first matching entity, or null if no match is found</returns>
        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves all entities that match the specified filter criteria.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="filter">Lambda expression defining the filter criteria</param>
        /// <param name="include">Array of navigation property names to include in the query (eager loading)</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better read performance (default: false)</param>
        /// <returns>Collection of entities matching the filter criteria</returns>
        Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a strongly-typed filter object that inherits from DefaultFilter.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="TFilter">The filter type that defines query criteria</typeparam>
        /// <param name="filter">The filter object containing query criteria</param>
        /// <param name="include">Array of navigation property names to include in the query (eager loading)</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better read performance (default: false)</param>
        /// <returns>Collection of entities matching the filter criteria</returns>
        Task<IEnumerable<TEntity>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves all entities that belong to a specific user (based on CreatorUserId).
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="userId">The ID of the user whose entities should be retrieved</param>
        /// <param name="include">Array of navigation property names to include in the query (eager loading)</param>
        /// <returns>Collection of entities belonging to the specified user</returns>
        Task<IEnumerable<TEntity>> GetByUser<TEntity>(int userId, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support using a filter expression.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="pagination">Pagination model containing page size, page number, and optional sorting</param>
        /// <param name="filter">Lambda expression defining the filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query (eager loading)</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better read performance (default: false)</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support using a strongly-typed filter object.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="TFilter">The filter type that defines query criteria and pagination</typeparam>
        /// <param name="filter">The filter object containing query criteria and pagination settings</param>
        /// <param name="includes">Array of navigation property names to include in the query (eager loading)</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better read performance (default: false)</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support filtered by user ownership.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="pagination">Pagination model containing page size, page number, and optional sorting</param>
        /// <param name="idUser">The ID of the user whose entities should be retrieved</param>
        /// <param name="includes">Array of navigation property names to include in the query (eager loading)</param>
        /// <param name="asNoTracking">Whether to disable change tracking for better read performance (default: false)</param>
        /// <returns>Paginated result containing user's entities, total count, and pagination metadata</returns>
        Task<PaginationModel<TEntity>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> pagination, int idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity identified by its primary key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="entity">The entity instance containing updated values</param>
        /// <param name="key">The primary key value of the entity to update</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates multiple entities in a single batch operation for improved performance.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <param name="entityList">The collection of entities to update</param>
        /// <param name="forceDetach">Whether to force detaching entities after update (default: false)</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateBatch<TEntity>(IEnumerable<TEntity> entityList, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes entities that match the specified filter criteria.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to delete</typeparam>
        /// <param name="filter">Lambda expression defining which entities to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if any entities were deleted, false otherwise</returns>
        Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes multiple entities in a single batch operation for improved performance.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to delete</typeparam>
        /// <param name="entityList">The collection of entities to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes a single entity identified by its primary key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to delete</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="key">The primary key value of the entity to delete</param>
        /// <param name="saveChanges">Whether to immediately save changes to the database (default: false)</param>
        /// <returns>True if the entity was deleted, false if not found</returns>
        Task<bool> DeleteByKey<TEntity, TKey>(TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Asynchronously saves all pending changes to the database.
        /// This commits the current unit of work and makes all changes permanent.
        /// </summary>
        /// <returns>Task representing the asynchronous save operation</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Rolls back all pending changes and reverts the context to its previous state.
        /// This is useful for undoing changes when an error occurs during a transaction.
        /// </summary>
        /// <returns>Task representing the asynchronous rollback operation</returns>
        Task Rollback();
    }
}