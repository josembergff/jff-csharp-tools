using System.Collections.Generic;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools9.Apresentation.Controllers
{
    /// <summary>
    /// Abstract base controller that provides standard CRUD operations for entities.
    /// Inherits from DefaultController to leverage token handling and response formatting.
    /// </summary>
    /// <typeparam name="TService">The service type that implements IDefaultService</typeparam>
    /// <typeparam name="TContext">The Entity Framework DbContext type</typeparam>
    /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
    public abstract class DefaultCRUDController<TService, TContext, TEntity> : DefaultController where TService : IDefaultService<TContext> where TContext : DbContext where TEntity : DefaultEntity<TEntity>, new()
    {
        /// <summary>
        /// Service instance for performing CRUD operations
        /// </summary>
        private readonly TService serviceCrud;

        /// <summary>
        /// Initializes a new instance of the DefaultCRUDController
        /// </summary>
        /// <param name="serviceCrud">Service instance for CRUD operations</param>
        /// <param name="logger">Logger instance for error and information logging</param>
        public DefaultCRUDController(TService serviceCrud, ILogger<DefaultController> logger) : base(logger)
        {
            this.serviceCrud = serviceCrud;
        }

        /// <summary>
        /// Gets all entities for the current authenticated user
        /// </summary>
        /// <returns>Collection of entities belonging to the current user</returns>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Gets entities for the current authenticated user with optional filtering
        /// </summary>
        /// <param name="filter">Filter criteria based on entity properties</param>
        /// <returns>Filtered collection of entities belonging to the current user</returns>
        [HttpGet]
        [Route("filter")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetFilter([FromQuery] TEntity filter)
        {
            var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken, entityFilter: filter);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Gets paginated entities for the current authenticated user
        /// </summary>
        /// <param name="filter">Pagination parameters including page size, current page, and optional filters</param>
        /// <returns>Paginated result containing entities, total count, and pagination metadata</returns>
        [HttpGet]
        [Route("pagination")]
        public virtual async Task<ActionResult<PaginationModel<TEntity>>> GetPagination([FromQuery] PaginationModel<TEntity> filter)
        {
            var returnObj = await serviceCrud.GetPaginated(filter, f => f.CreatorUserId == CurrentIdUser_FromBearerToken);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Gets a specific entity by its key for the current authenticated user
        /// </summary>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <returns>The entity with the specified key, or null if not found or not owned by current user</returns>
        [HttpGet("{key}")]
        public virtual async Task<ActionResult<TEntity>> Get(int key)
        {
            var returnObj = await serviceCrud.GetByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Creates a new entity for the current authenticated user
        /// </summary>
        /// <param name="value">The entity data to create</param>
        /// <returns>The ID of the newly created entity</returns>
        [HttpPost]
        public virtual async Task<ActionResult<int>> Post([FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.Create(CurrentIdUser_FromBearerToken, value);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Updates an existing entity by its key for the current authenticated user
        /// </summary>
        /// <param name="key">The primary key of the entity to update</param>
        /// <param name="value">The updated entity data</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        [HttpPut("{key}")]
        public virtual async Task<ActionResult<bool>> Put(int key, [FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.UpdateByKey(CurrentIdUser_FromBearerToken, value, key);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// Deletes an entity by its key for the current authenticated user
        /// </summary>
        /// <param name="key">The primary key of the entity to delete</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        [HttpDelete("{key}")]
        public virtual async Task<ActionResult<bool>> Delete(int key)
        {
            var returnObj = await serviceCrud.DeleteByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }
    }
}