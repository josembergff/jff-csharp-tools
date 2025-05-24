using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Domain.Interface.Repository;
using JffCsharpTools9.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Service
{
    public class DefaultService<T> : IDefaultService<T> where T : DbContext
    {
        public IDefaultRepository<T> defaultRepository { get; set; }

        public DefaultService(IDefaultRepository<T> defaultRepository)
        {
            this.defaultRepository = defaultRepository;
        }

        public virtual async Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new()
        {
            var idReturn = new DefaultResponseModel<int>() { Result = 0 };
            entity.CreatedAt = DateTime.Now;
            entity.CreatorUserId = IdUser;
            var returnCreate = await defaultRepository.Create(entity);
            idReturn.Result = returnCreate.Id;
            return idReturn;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userFilterObjBase = await defaultRepository.Get(entityFilter.GetFilter(), includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByUser<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userObjBase = await defaultRepository.GetByUser<TEntity>(IdUser, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userObjBase = await defaultRepository.GetByFilter<TEntity, TFilter>(filter, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<TEntity>> GetByKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<TEntity>();
            var userObjBase = await defaultRepository.GetByKey<TEntity, Tkey>(key, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            var userFilterObjBase = await defaultRepository.GetPaginated(pagination, filter, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            if (filter == null)
            {
                filter = new TFilter();
            }
            var userFilterObjBase = await defaultRepository.GetPaginatedByFilter<TEntity, TFilter>(filter, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> paginacao, int IdUser, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            var userFilterObjBase = await defaultRepository.GetPaginatedByUser(paginacao, IdUser, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<bool>> DeleteByKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var userObjBase = await defaultRepository.GetByKey<TEntity, TKey>(key);
            if (userObjBase != null)
            {
                returnValue.Result = await defaultRepository.DeleteByKey<TEntity, TKey>(key);
            }
            return returnValue;
        }
        public virtual async Task<DefaultResponseModel<bool>> UpdateByKey<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var entityObjBase = await defaultRepository.GetByKey<TEntity, TKey>(key);
            entity.UpdatedAt = DateTime.Now;
            if (entityObjBase != null)
            {
                returnValue.Result = await defaultRepository.UpdateByKey(entity, key);
            }
            return returnValue;
        }
    }
}