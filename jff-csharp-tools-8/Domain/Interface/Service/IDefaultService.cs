using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools8.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Domain.Interface.Service
{
    /// <summary>
    /// Service interface that provides business logic layer operations for entities.
    /// This interface acts as an intermediary between controllers and repositories, encapsulating business rules and validation.
    /// All operations return standardized DefaultResponseModel objects containing success status, data, and error information.
    /// Operations are user-scoped, ensuring data isolation and security based on user authentication.
    /// </summary>
    /// <typeparam name="T">The DbContext type used for database operations</typeparam>
    public interface IDefaultService<T> where T : DbContext
    {
        /// <summary>
        /// Gets or sets the repository instance used for data access operations.
        /// This property provides access to the underlying data layer for performing CRUD operations.
        /// </summary>
        IDefaultRepository<T> defaultRepository { get; set; }

        /// <summary>
        /// Creates a new entity in the system with user context.
        /// Validates business rules and assigns the creating user as the owner of the entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to create</typeparam>
        /// <param name="IdUser">The ID of the user creating the entity (used for auditing and ownership)</param>
        /// <param name="entity">The entity instance to create</param>
        /// <returns>Response model containing the ID of the created entity or error information</returns>
        Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with optional filtering, without user-based restrictions.
        /// This method is typically used for administrative or system-level operations.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="entityFilter">Optional entity filter to apply to the query (default: null)</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing the collection of entities or error information</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities that belong to a specific user with optional filtering.
        /// Ensures data isolation by only returning entities owned by the specified user.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="IdUser">The ID of the user whose entities should be retrieved</param>
        /// <param name="entityFilter">Optional entity filter to apply to the query (default: null)</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing the user's entities or error information</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByUser<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a strongly-typed filter object that inherits from DefaultFilter.
        /// Provides advanced filtering capabilities with type safety.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="TFilter">The filter type that defines query criteria</typeparam>
        /// <param name="filter">The filter object containing query criteria and configuration</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing the filtered entities or error information</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves a single entity by its primary key with user validation.
        /// Ensures that the requesting user has access to the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="Tkey">The primary key type</typeparam>
        /// <param name="IdUser">The ID of the user requesting the entity</param>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing the requested entity or error information</returns>
        Task<DefaultResponseModel<TEntity>> GetByKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support using a filter expression.
        /// Provides efficient data retrieval for large datasets with paging capabilities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="pagination">Pagination model containing page size, page number, and optional sorting</param>
        /// <param name="filter">Lambda expression defining the filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing paginated results with metadata or error information</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support using a strongly-typed filter object.
        /// Combines advanced filtering with pagination for complex query scenarios.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <typeparam name="TFilter">The filter type that defines query criteria and pagination</typeparam>
        /// <param name="filter">The filter object containing query criteria and pagination settings</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing paginated filtered results or error information</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support filtered by user ownership.
        /// Provides paginated results while maintaining user-based data isolation.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to retrieve</typeparam>
        /// <param name="paginacao">Pagination model containing page size, page number, and optional sorting</param>
        /// <param name="IdUser">The ID of the user whose entities should be retrieved</param>
        /// <param name="includes">Array of navigation property names to include in the query (default: null)</param>
        /// <returns>Response model containing paginated user entities or error information</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> paginacao, int IdUser, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity identified by its primary key with user validation.
        /// Validates business rules and ensures the user has permission to update the entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to update</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="IdUser">The ID of the user performing the update</param>
        /// <param name="entity">The entity instance containing updated values</param>
        /// <param name="key">The primary key value of the entity to update</param>
        /// <returns>Response model containing boolean indicating success or error information</returns>
        Task<DefaultResponseModel<bool>> UpdateByKey<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes an entity identified by its primary key with user validation.
        /// Validates that the user has permission to delete the entity before performing the operation.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to delete</typeparam>
        /// <typeparam name="TKey">The primary key type</typeparam>
        /// <param name="IdUser">The ID of the user performing the deletion</param>
        /// <param name="key">The primary key value of the entity to delete</param>
        /// <returns>Response model containing boolean indicating success or error information</returns>
        Task<DefaultResponseModel<bool>> DeleteByKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new();
    }
}