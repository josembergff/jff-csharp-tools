
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools8.Domain.Interface.Repository;
using JffCsharpTools8.Domain.Interface.Service;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools8.Domain.Service
{
    public class DefaultService<T> : IDefaultService<T> where T : DbContext
    {
        public IDefaultRepository<T> defaultRepository { get; set; }

        public DefaultService(IDefaultRepository<T> defaultRepository)
        {
            this.defaultRepository = defaultRepository;
        }

        public async Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new()
        {
            var idReturn = new DefaultResponseModel<int>() { Result = 0 };
            entity.CreatedAt = DateTime.Now;
            entity.CreatorUserId = IdUser;
            var returnCreate = await defaultRepository.Create(entity);
            idReturn.Result = returnCreate.Id;
            return idReturn;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            if (entityFilter != null)
            {
                entityFilter.CreatorUserId = IdUser;
                var userFilterObjBase = await defaultRepository.Get(entityFilter.GetFilter(), includes);
                if (userFilterObjBase != null)
                {
                    returnValue.Result = userFilterObjBase.ToList();
                }
            }
            else
            {
                var userObjBase = await defaultRepository.GetUser<TEntity>(IdUser);
                if (userObjBase != null)
                {
                    returnValue.Result = userObjBase.ToList();
                }
            }
            return returnValue;
        }

        public async Task<DefaultResponseModel<TEntity>> GetKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<TEntity>();
            var userObjBase = await defaultRepository.GetKey<TEntity, Tkey>(key, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase;
            }
            return returnValue;
        }

        public async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPagination<TEntity>(int IdUser, PaginationModel<TEntity> paginacao, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            if (paginacao.Filter == null)
            {
                paginacao.Filter = new DefaultFilter();
            }
            paginacao.Filter.CreatorUserId = IdUser;
            var userFilterObjBase = await defaultRepository.GetPagination(paginacao, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public async Task<DefaultResponseModel<bool>> DeleteKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var userObjBase = await defaultRepository.GetKey<TEntity, TKey>(key);
            if (userObjBase != null)
            {
                returnValue.Result = await defaultRepository.DeleteKey<TEntity, TKey>(key);
            }
            return returnValue;
        }
        public async Task<DefaultResponseModel<bool>> Update<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var entityObjBase = await defaultRepository.GetKey<TEntity, TKey>(key);
            entity.UpdatedAt = DateTime.Now;
            if (entityObjBase != null)
            {
                returnValue.Result = await defaultRepository.UpdateKey(entity, key);
            }
            return returnValue;
        }
    }
}