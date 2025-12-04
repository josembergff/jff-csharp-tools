using System.Collections.Generic;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Model;
using JffCsharpTools8.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JffCsharpTools8.Apresentation.Controllers
{
    /// <summary>
    /// Abstract base controller providing standard CRUD operations for entities
    /// Implements common HTTP endpoints (GET, POST, PUT, DELETE) with user-based access control
    /// Inherits from DefaultController to leverage JWT token functionality
    /// </summary>
    /// <typeparam name="TService">The service type that implements IDefaultService interface</typeparam>
    /// <typeparam name="TContext">The DbContext type used for database operations</typeparam>
    /// <typeparam name="TEntity">The entity type that inherits from DefaultEntity</typeparam>
    public abstract class DefaultCRUDController<TService, TContext, TEntity> : DefaultController
        where TService : IDefaultService<TContext>
        where TContext : DbContext
        where TEntity : DefaultEntity<TEntity>, new()
    {
        /// <summary>
        /// The service instance used for performing CRUD operations
        /// </summary>
        private readonly TService serviceCrud;
        private readonly bool filterCurrentUser;

        /// <summary>
        /// Initializes a new instance of the DefaultCRUDController
        /// </summary>
        /// <param name="serviceCrud">The service instance for CRUD operations</param>
        /// <param name="logger">Logger instance for recording application events and errors</param>
        public DefaultCRUDController(TService serviceCrud, ILogger<DefaultController> logger, bool filterCurrentUser = true) : base(logger)
        {
            this.serviceCrud = serviceCrud;
            this.filterCurrentUser = filterCurrentUser;
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve all entities belonging to the current user
        /// </summary>
        /// <returns>Collection of entities belonging to the authenticated user</returns>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            if (filterCurrentUser)
            {
                var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken);
                return ReturnAction(returnObj);
            }
            else
            {
                var returnObj = await serviceCrud.Get<TEntity>();
                return ReturnAction(returnObj);
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve entities with filtering capabilities
        /// Allows filtering by entity properties passed as query parameters
        /// </summary>
        /// <param name="filter">Entity filter containing search criteria from query parameters</param>
        /// <returns>Collection of filtered entities belonging to the authenticated user</returns>
        [HttpGet]
        [Route("filter")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetFilter([FromQuery] TEntity filter)
        {
            if (filterCurrentUser)
            {
                var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken, entityFilter: filter);
                return ReturnAction(returnObj);
            }
            else
            {
                var returnObj = await serviceCrud.Get<TEntity>(entityFilter: filter);
                return ReturnAction(returnObj);
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve entities with pagination support
        /// Supports page size, page number, sorting, and filtering
        /// </summary>
        /// <param name="filter">Pagination model containing page parameters and optional entity filter</param>
        /// <returns>Paginated results with metadata including total count and page information</returns>
        [HttpGet]
        [Route("pagination")]
        public virtual async Task<ActionResult<PaginationModel<TEntity>>> GetPagination([FromQuery] PaginationModel<TEntity> filter)
        {
            var returnObj = await serviceCrud.GetPaginated(filter, f => true, IdUser: CurrentIdUser_FromBearerToken, filterCurrentUser: filterCurrentUser);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve a single entity by its primary key
        /// Validates that the entity belongs to the authenticated user
        /// </summary>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <returns>The requested entity if found and accessible by the user</returns>
        [HttpGet("{key}")]
        public virtual async Task<ActionResult<TEntity>> Get(int key)
        {
            var returnObj = await serviceCrud.GetByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key, filterCurrentUser: filterCurrentUser);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// HTTP POST endpoint to create a new entity
        /// Associates the entity with the authenticated user as creator
        /// </summary>
        /// <param name="value">The entity data to create from the request body</param>
        /// <returns>The ID of the newly created entity</returns>
        [HttpPost]
        public virtual async Task<ActionResult<int>> Post([FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.Create(CurrentIdUser_FromBearerToken, value, filterCurrentUser: filterCurrentUser);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// HTTP PUT endpoint to update an existing entity by its primary key
        /// Validates that the entity belongs to the authenticated user before updating
        /// </summary>
        /// <param name="key">The primary key value of the entity to update</param>
        /// <param name="value">The updated entity data from the request body</param>
        /// <returns>Boolean indicating whether the update was successful</returns>
        [HttpPut("{key}")]
        public virtual async Task<ActionResult<bool>> Put(int key, [FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.UpdateByKey(CurrentIdUser_FromBearerToken, value, key, filterCurrentUser: filterCurrentUser);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// HTTP DELETE endpoint to delete an entity by its primary key
        /// Validates that the entity belongs to the authenticated user before deletion
        /// </summary>
        /// <param name="key">The primary key value of the entity to delete</param>
        /// <returns>Boolean indicating whether the deletion was successful</returns>
        [HttpDelete("{key}")]
        public virtual async Task<ActionResult<bool>> Delete(int key)
        {
            var returnObj = await serviceCrud.DeleteByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key, filterCurrentUser: filterCurrentUser);
            return ReturnAction(returnObj);
        }
    }
}