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
    /// Abstract base controller that provides standard CRUD (Create, Read, Update, Delete) operations.
    /// This controller implements common REST API endpoints for entities that inherit from DefaultEntity.
    /// All CRUD operations are user-scoped, meaning they only operate on data belonging to the authenticated user.
    /// Controllers should inherit from this class to get standard CRUD functionality with minimal code.
    /// </summary>
    /// <typeparam name="TService">The service type that handles business logic and data access</typeparam>
    /// <typeparam name="TContext">The Entity Framework DbContext type used for database operations</typeparam>
    /// <typeparam name="TEntity">The entity type that represents the data model being managed</typeparam>
    public abstract class DefaultCRUDController<TService, TContext, TEntity> : DefaultController
        where TService : IDefaultService<TContext>
        where TContext : DbContext
        where TEntity : DefaultEntity<TEntity>, new()
    {
        /// <summary>
        /// The service instance responsible for handling business logic and data operations
        /// </summary>
        private readonly TService serviceCrud;

        /// <summary>
        /// Initializes a new instance of the DefaultCRUDController class.
        /// </summary>
        /// <param name="serviceCrud">The service instance that will handle CRUD operations</param>
        /// <param name="logger">The logger instance for logging errors and information</param>
        public DefaultCRUDController(TService serviceCrud, ILogger<DefaultController> logger) : base(logger)
        {
            this.serviceCrud = serviceCrud;
        }

        /// <summary>
        /// GET endpoint that retrieves all entities belonging to the authenticated user.
        /// Returns a collection of entities filtered by the current user's ID from the JWT token.
        /// </summary>
        /// <returns>ActionResult containing a collection of entities owned by the current user</returns>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// GET endpoint with filtering capabilities that retrieves entities based on filter criteria.
        /// Applies the provided filter object to narrow down results while maintaining user scope.
        /// </summary>
        /// <param name="filter">The filter object containing criteria to apply when querying entities</param>
        /// <returns>ActionResult containing filtered entities owned by the current user</returns>
        [HttpGet]
        [Route("filter")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetFilter([FromQuery] TEntity filter)
        {
            var returnObj = await serviceCrud.GetByUser<TEntity>(CurrentIdUser_FromBearerToken, entityFilter: filter);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// GET endpoint that provides paginated results for entities belonging to the authenticated user.
        /// Supports pagination parameters like page number, page size, and optional filtering.
        /// </summary>
        /// <param name="filter">The pagination model containing page size, page number, and optional filter criteria</param>
        /// <returns>ActionResult containing paginated results with metadata (total count, page info, etc.)</returns>
        [HttpGet]
        [Route("pagination")]
        public virtual async Task<ActionResult<PaginationModel<TEntity>>> GetPagination([FromQuery] PaginationModel<TEntity> filter)
        {
            var returnObj = await serviceCrud.GetPaginated(filter, f => f.CreatorUserId == CurrentIdUser_FromBearerToken);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// GET endpoint that retrieves a specific entity by its primary key.
        /// Validates that the entity belongs to the authenticated user before returning it.
        /// </summary>
        /// <param name="key">The primary key value of the entity to retrieve</param>
        /// <returns>ActionResult containing the requested entity if found and owned by current user</returns>
        [HttpGet("{key}")]
        public virtual async Task<ActionResult<TEntity>> Get(int key)
        {
            var returnObj = await serviceCrud.GetByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// POST endpoint that creates a new entity in the system.
        /// Automatically assigns the authenticated user as the creator/owner of the entity.
        /// </summary>
        /// <param name="value">The entity object to create in the database</param>
        /// <returns>ActionResult containing the ID of the newly created entity</returns>
        [HttpPost]
        public virtual async Task<ActionResult<int>> Post([FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.Create(CurrentIdUser_FromBearerToken, value);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// PUT endpoint that updates an existing entity identified by its primary key.
        /// Validates that the entity belongs to the authenticated user before allowing the update.
        /// </summary>
        /// <param name="key">The primary key of the entity to update</param>
        /// <param name="value">The entity object containing updated values</param>
        /// <returns>ActionResult containing boolean indicating success or failure of the update operation</returns>
        [HttpPut("{key}")]
        public virtual async Task<ActionResult<bool>> Put(int key, [FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.UpdateByKey(CurrentIdUser_FromBearerToken, value, key);
            return ReturnAction(returnObj);
        }

        /// <summary>
        /// DELETE endpoint that removes an entity identified by its primary key.
        /// Validates that the entity belongs to the authenticated user before allowing deletion.
        /// </summary>
        /// <param name="key">The primary key of the entity to delete</param>
        /// <returns>ActionResult containing boolean indicating success or failure of the delete operation</returns>
        [HttpDelete("{key}")]
        public virtual async Task<ActionResult<bool>> Delete(int key)
        {
            var returnObj = await serviceCrud.DeleteByKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }
    }
}