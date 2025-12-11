
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
    public class DefaultGuidService<T> : IDefaultGuidService<T> where T : DbContext
    {
        public IDefaultGuidRepository<T> defaultGuidRepository { get; set; }

        public DefaultGuidService(IDefaultGuidRepository<T> defaultGuidRepository)
        {
            this.defaultGuidRepository = defaultGuidRepository;
        }

        public virtual async Task<DefaultResponseModel<int>> Create<TEntity>(Guid IdUser, TEntity entity, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var idReturn = new DefaultResponseModel<int>() { Result = 0 };
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatorUserId = !filterCurrentUser && entity.CreatorUserId != Guid.Empty ? entity.CreatorUserId : IdUser;
            var returnCreate = await defaultGuidRepository.Create(entity);
            idReturn.Result = returnCreate.Id;
            return idReturn;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userFilterObjBase = await defaultGuidRepository.Get(entityFilter.GetFilter(), includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByUser<TEntity>(Guid IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userObjBase = await defaultGuidRepository.GetByUser<TEntity>(IdUser, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<IEnumerable<TEntity>>();

            var userObjBase = await defaultGuidRepository.GetByFilter<TEntity, TFilter>(filter, includes);
            if (userObjBase != null)
            {
                returnValue.Result = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<TEntity>> GetByKey<TEntity, Tkey>(Guid IdUser, Tkey key, string[] includes = null, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<TEntity>();
            var userObjBase = await defaultGuidRepository.GetByKey<TEntity, Tkey>(key, includes);
            if (userObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (userObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Result = userObjBase;
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to access this entity.";
                        returnValue.Result = null;
                    }
                }
                else
                {
                    returnValue.Result = userObjBase;
                }
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool filterCurrentUser = true, Guid IdUser = default) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            var userFilterObjBase = await defaultGuidRepository.GetPaginated(pagination, filter, includes);
            if (userFilterObjBase != null)
            {
                if (filterCurrentUser)
                {
                    userFilterObjBase.List = userFilterObjBase.List.Where(e => e.CreatorUserId == IdUser).ToList();
                }
                else
                {
                    returnValue.Result = userFilterObjBase;
                }
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            if (filter == null)
            {
                filter = new TFilter();
            }
            var userFilterObjBase = await defaultGuidRepository.GetPaginatedByFilter<TEntity, TFilter>(filter, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> paginacao, Guid IdUser, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<PaginationModel<TEntity>>();
            var userFilterObjBase = await defaultGuidRepository.GetPaginatedByUser(paginacao, IdUser, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Result = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<DefaultResponseModel<bool>> DeleteByKey<TEntity, TKey>(Guid IdUser, TKey key, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var userObjBase = await defaultGuidRepository.GetByKey<TEntity, TKey>(key);
            if (userObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (userObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Result = await defaultGuidRepository.DeleteByKey<TEntity, TKey>(key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to delete this entity.";
                        returnValue.Result = false;
                    }
                }
                else
                {
                    returnValue.Result = await defaultGuidRepository.DeleteByKey<TEntity, TKey>(key);
                }
            }
            return returnValue;
        }
        public virtual async Task<DefaultResponseModel<bool>> UpdateByKey<TEntity, TKey>(Guid IdUser, TEntity entity, TKey key, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new DefaultResponseModel<bool>() { Result = false };
            var entityObjBase = await defaultGuidRepository.GetByKey<TEntity, TKey>(key);
            entity.UpdatedAt = DateTime.UtcNow;
            if (entityObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (entityObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Result = await defaultGuidRepository.UpdateByKey(entity, key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to update this entity.";
                        returnValue.Result = false;
                    }
                }
                else
                {
                    returnValue.Result = await defaultGuidRepository.UpdateByKey(entity, key);
                }
            }
            return returnValue;
        }
    }
}