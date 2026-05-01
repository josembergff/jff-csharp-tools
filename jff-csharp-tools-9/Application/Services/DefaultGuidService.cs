
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Application.Common;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Extensions;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Common;
using JffCsharpTools9.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;
using JffCsharpTools9.Application.Interfaces;

namespace JffCsharpTools9.Application.Services
{
    public class DefaultGuidService<T> : IDefaultGuidService<T> where T : DbContext
    {
        public IDefaultGuidRepository<T> defaultGuidRepository { get; set; }

        public DefaultGuidService(IDefaultGuidRepository<T> defaultGuidRepository)
        {
            this.defaultGuidRepository = defaultGuidRepository;
        }

        public virtual async Task<Result<int>> Create<TEntity>(Guid IdUser, TEntity entity, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            entity = entity.ConvertDatesToUtc();
            var idReturn = new Result<int>() { Value = 0 };
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatorUserId = !filterCurrentUser && entity.CreatorUserId != Guid.Empty ? entity.CreatorUserId : IdUser;
            var returnCreate = await defaultGuidRepository.Create(entity);
            idReturn.Value = returnCreate.Id;
            return idReturn;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userFilterObjBase = await defaultGuidRepository.Get(entityFilter.GetFilter(), includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetByUser<TEntity>(Guid IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userObjBase = await defaultGuidRepository.GetByUser<TEntity>(IdUser, includes);
            if (userObjBase != null)
            {
                returnValue.Value = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userObjBase = await defaultGuidRepository.GetByFilter<TEntity, TFilter>(filter, includes);
            if (userObjBase != null)
            {
                returnValue.Value = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<TEntity>> GetByKey<TEntity, Tkey>(Guid IdUser, Tkey key, string[] includes = null, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<TEntity>();
            var userObjBase = await defaultGuidRepository.GetByKey<TEntity, Tkey>(key, includes);
            if (userObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (userObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Value = userObjBase;
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to access this entity.";
                        returnValue.Value = null;
                    }
                }
                else
                {
                    returnValue.Value = userObjBase;
                }
            }
            return returnValue;
        }

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginated<TEntity>(PaginationResult<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool filterCurrentUser = true, Guid IdUser = default) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            var userFilterObjBase = await defaultGuidRepository.GetPaginated(pagination, filter, includes);
            if (userFilterObjBase != null)
            {
                if (filterCurrentUser)
                {
                    userFilterObjBase.List = userFilterObjBase.List.Where(e => e.CreatorUserId == IdUser).ToList();
                }
                else
                {
                    returnValue.Value = userFilterObjBase;
                }
            }
            return returnValue;
        }

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            if (filter == null)
            {
                filter = new TFilter();
            }
            var userFilterObjBase = await defaultGuidRepository.GetPaginatedByFilter<TEntity, TFilter>(filter, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedByUser<TEntity>(PaginationResult<TEntity> paginacao, Guid IdUser, string[] includes = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            var userFilterObjBase = await defaultGuidRepository.GetPaginatedByUser(paginacao, IdUser, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<Result<bool>> DeleteByKey<TEntity, TKey>(Guid IdUser, TKey key, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<bool>() { Value = false };
            var userObjBase = await defaultGuidRepository.GetByKey<TEntity, TKey>(key);
            if (userObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (userObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Value = await defaultGuidRepository.DeleteByKey<TEntity, TKey>(key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to delete this entity.";
                        returnValue.Value = false;
                    }
                }
                else
                {
                    returnValue.Value = await defaultGuidRepository.DeleteByKey<TEntity, TKey>(key);
                }
            }
            return returnValue;
        }
        public virtual async Task<Result<bool>> UpdateByKey<TEntity, TKey>(Guid IdUser, TEntity entity, TKey key, bool filterCurrentUser = true) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            var returnValue = new Result<bool>() { Value = false };
            entity = entity.ConvertDatesToUtc();
            var entityObjBase = await defaultGuidRepository.GetByKey<TEntity, TKey>(key);
            entity.UpdatedAt = DateTime.UtcNow;
            if (entityObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (entityObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Value = await defaultGuidRepository.UpdateByKey(entity, key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to update this entity.";
                        returnValue.Value = false;
                    }
                }
                else
                {
                    returnValue.Value = await defaultGuidRepository.UpdateByKey(entity, key);
                }
            }
            return returnValue;
        }
    }
}