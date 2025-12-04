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
    /// Default service interface providing CRUD operations for entities with user-based access control
    /// This interface defines the contract for services that handle database operations through a repository pattern
    /// </summary>
    /// <typeparam name="T">The DbContext type that will be used for database operations</typeparam>
    public interface IDefaultService<T> where T : DbContext
    {
        /// <summary>
        /// Gets or sets the default repository instance used for database operations
        /// </summary>
        IDefaultRepository<T> defaultRepository { get; set; }

        /// <summary>
        /// Creates a new entity in the database with user tracking
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="IdUser">The ID of the user creating the entity for auditing purposes</param>
        /// <param name="entity">The entity instance to be created</param>
        /// <returns>A response model containing the ID of the created entity</returns>
        Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity, bool filterCurrentUser = true) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves all entities with optional filtering and including related data
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityFilter">Optional entity filter to apply search criteria</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the collection of entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities filtered by user ID with optional additional filtering
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="IdUser">The user ID to filter entities by (usually CreatorUserId)</param>
        /// <param name="entityFilter">Optional entity filter to apply additional search criteria</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the collection of user-specific entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByUser<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a custom filter object that extends DefaultFilter
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter for the entity</typeparam>
        /// <param name="filter">The filter object containing search criteria and parameters</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the filtered collection of entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves a single entity by its primary key with user access validation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="Tkey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="IdUser">The user ID to validate access rights to the entity</param>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the requested entity or null if not found/not accessible</returns>
        Task<DefaultResponseModel<TEntity>> GetByKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null, bool filterCurrentUser = true) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination support and custom filtering
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">The pagination model containing page size, page number, and ordering information</param>
        /// <param name="filter">Lambda expression to filter the entities</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the paginated results with metadata</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool filterCurrentUser = true, int IdUser = 0) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination using a custom filter object
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter and includes pagination</typeparam>
        /// <param name="filter">The filter object containing search criteria and pagination parameters</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the paginated and filtered results</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves entities with pagination filtered by user ID
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="paginacao">The pagination model containing page size, page number, and ordering information</param>
        /// <param name="IdUser">The user ID to filter entities by (usually CreatorUserId)</param>
        /// <param name="includes">Optional array of navigation property names to include in the query</param>
        /// <returns>A response model containing the paginated user-specific results</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> paginacao, int IdUser, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity by its primary key with user validation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="IdUser">The user ID performing the update for auditing and access validation</param>
        /// <param name="entity">The entity instance with updated values</param>
        /// <param name="key">The primary key value of the entity to update</param>
        /// <returns>A response model containing a boolean indicating success or failure</returns>
        Task<DefaultResponseModel<bool>> UpdateByKey<TEntity, TKey>(int IdUser, TEntity entity, TKey key, bool filterCurrentUser = true) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes an entity by its primary key with user validation
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key (int, string, Guid, etc.)</typeparam>
        /// <param name="IdUser">The user ID performing the deletion for auditing and access validation</param>
        /// <param name="key">The primary key value of the entity to delete</param>
        /// <returns>A response model containing a boolean indicating success or failure</returns>
        Task<DefaultResponseModel<bool>> DeleteByKey<TEntity, TKey>(int IdUser, TKey key, bool filterCurrentUser = true) where TEntity : DefaultEntity<TEntity>, new();
    }

}