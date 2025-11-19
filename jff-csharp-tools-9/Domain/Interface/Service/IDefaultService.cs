using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Interface.Service
{
    /// <summary>
    /// Generic service interface that provides business logic layer operations for CRUD functionality.
    /// Acts as an intermediary between controllers and repositories, handling business rules,
    /// validation, and returning standardized response models.
    /// </summary>
    /// <typeparam name="T">The Entity Framework DbContext type</typeparam>
    public interface IDefaultService<T> where T : DbContext
    {
        /// <summary>
        /// Gets or sets the default repository instance for data access operations
        /// </summary>
        IDefaultRepository<T> defaultRepository { get; set; }

        /// <summary>
        /// Creates a new entity with user ownership tracking
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="IdUser">The ID of the user creating the entity</param>
        /// <param name="entity">The entity instance to create</param>
        /// <returns>Standardized response containing the ID of the created entity</returns>
        Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities with optional filtering capabilities
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="entityFilter">Optional entity filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing collection of entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities that belong to a specific user with optional filtering
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="IdUser">The ID of the user who owns the entities</param>
        /// <param name="entityFilter">Optional entity filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing collection of user's entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByUser<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves entities using a typed filter object that defines search criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter</typeparam>
        /// <param name="filter">The filter object containing search criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing filtered collection of entities</returns>
        Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves a specific entity by its primary key, ensuring user ownership
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="Tkey">The type of the primary key</typeparam>
        /// <param name="IdUser">The ID of the user requesting the entity</param>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing the requested entity</returns>
        Task<DefaultResponseModel<TEntity>> GetByKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set of entities that match the specified filter criteria
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="pagination">Pagination parameters including page size and current page</param>
        /// <param name="filter">Expression defining the filter criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing paginated entities with metadata</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set using a typed filter object
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TFilter">The filter type that inherits from DefaultFilter</typeparam>
        /// <param name="filter">The filter object containing search and pagination criteria</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing paginated entities with metadata</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new();

        /// <summary>
        /// Retrieves a paginated result set of entities that belong to a specific user
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <param name="paginacao">Pagination parameters including page size and current page</param>
        /// <param name="IdUser">The ID of the user who owns the entities</param>
        /// <param name="includes">Array of navigation property names to include in the query</param>
        /// <returns>Standardized response containing paginated user entities with metadata</returns>
        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> paginacao, int IdUser, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Updates an existing entity by its primary key, ensuring user ownership
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key</typeparam>
        /// <param name="IdUser">The ID of the user performing the update</param>
        /// <param name="entity">The entity with updated values</param>
        /// <param name="key">The primary key of the entity to update</param>
        /// <returns>Standardized response indicating success or failure of the update operation</returns>
        Task<DefaultResponseModel<bool>> UpdateByKey<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new();

        /// <summary>
        /// Deletes an entity by its primary key, ensuring user ownership
        /// </summary>
        /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
        /// <typeparam name="TKey">The type of the primary key</typeparam>
        /// <param name="IdUser">The ID of the user performing the deletion</param>
        /// <param name="key">The primary key of the entity to delete</param>
        /// <returns>Standardized response indicating success or failure of the delete operation</returns>
        Task<DefaultResponseModel<bool>> DeleteByKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new();
    }

}