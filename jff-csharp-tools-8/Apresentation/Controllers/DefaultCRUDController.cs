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
    public abstract class DefaultCRUDController<TService, TContext, TEntity> : DefaultController where TService : IDefaultService<TContext> where TContext : DbContext where TEntity : DefaultEntity<TEntity>, new()
    {
        private readonly TService serviceCrud;

        public DefaultCRUDController(TService serviceCrud, ILogger<DefaultController> logger) : base(logger)
        {
            this.serviceCrud = serviceCrud;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            var returnObj = await serviceCrud.Get<TEntity>(CurrentIdUser_FromBearerToken);
            return ReturnAction(returnObj);
        }

        [HttpGet]
        [Route("filter")]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetFilter([FromQuery] TEntity filter)
        {
            var returnObj = await serviceCrud.Get<TEntity>(CurrentIdUser_FromBearerToken, entityFilter: filter);
            return ReturnAction(returnObj);
        }

        [HttpGet]
        [Route("pagination")]
        public virtual async Task<ActionResult<PaginationModel<TEntity>>> GetPagination([FromQuery] PaginationModel<TEntity> filter)
        {
            var returnObj = await serviceCrud.GetPagination(CurrentIdUser_FromBearerToken, filter);
            return ReturnAction(returnObj);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<TEntity>> Get(int key)
        {
            var returnObj = await serviceCrud.GetKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }

        [HttpPost]
        public virtual async Task<ActionResult<int>> Post([FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.Create(CurrentIdUser_FromBearerToken, value);
            return ReturnAction(returnObj);
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<bool>> Put(int key, [FromBody] TEntity value)
        {
            var returnObj = await serviceCrud.Update(CurrentIdUser_FromBearerToken, value, key);
            return ReturnAction(returnObj);
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<bool>> Delete(int key)
        {
            var returnObj = await serviceCrud.DeleteKey<TEntity, int>(CurrentIdUser_FromBearerToken, key);
            return ReturnAction(returnObj);
        }
    }
}