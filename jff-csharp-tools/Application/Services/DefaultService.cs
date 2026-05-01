using System.Linq.Expressions;
using JffCsharpTools.Application.Common;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Extensions;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Common;
using JffCsharpTools.Application.Interfaces;
using JffCsharpTools.Domain.Interfaces.Repositories;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JffCsharpTools.Application.Services
{
    public class DefaultService : IDefaultService
    {
        public IDefaultRepository defaultRepository { get; set; }

        public DefaultService(IDefaultRepository defaultRepository)
        {
            this.defaultRepository = defaultRepository;
        }

        public virtual async Task<Result<int>> Create<TEntity>(int IdUser, TEntity entity, bool filterCurrentUser = true) where TEntity : DefaultEntity, new()
        {
            var idReturn = new Result<int>() { Value = 0 };
            entity = entity.ConvertDatesToUtc();
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatorUserId = !filterCurrentUser && entity.CreatorUserId > 0 ? entity.CreatorUserId : IdUser;
            var returnCreate = await defaultRepository.Create(entity);
            idReturn.Value = returnCreate.Id;
            return idReturn;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> Get<TEntity>(TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userFilterObjBase = await defaultRepository.Get(entityFilter.GetFilter<TEntity>(), includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetByUser<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userObjBase = await defaultRepository.GetByUser<TEntity>(IdUser, includes);
            if (userObjBase != null)
            {
                returnValue.Value = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<IEnumerable<TEntity>>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new Result<IEnumerable<TEntity>>();

            var userObjBase = await defaultRepository.GetByFilter<TEntity, TFilter>(filter, includes);
            if (userObjBase != null)
            {
                returnValue.Value = userObjBase.ToList();
            }
            return returnValue;
        }

        public virtual async Task<Result<TEntity>> GetByKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null, bool filterCurrentUser = true) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<TEntity>();
            var userObjBase = await defaultRepository.GetByKey<TEntity, Tkey>(key, includes);
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

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginated<TEntity>(PaginationResult<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool filterCurrentUser = true, int IdUser = 0) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            var userFilterObjBase = await defaultRepository.GetPaginated(pagination, filter, includes);
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

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null) where TEntity : DefaultEntity, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            if (filter == null)
            {
                filter = new TFilter();
            }
            var userFilterObjBase = await defaultRepository.GetPaginatedByFilter<TEntity, TFilter>(filter, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<Result<PaginationResult<TEntity>>> GetPaginatedByUser<TEntity>(PaginationResult<TEntity> paginacao, int IdUser, string[] includes = null) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<PaginationResult<TEntity>>();
            var userFilterObjBase = await defaultRepository.GetPaginatedByUser(paginacao, IdUser, includes);
            if (userFilterObjBase != null)
            {
                returnValue.Value = userFilterObjBase;
            }
            return returnValue;
        }

        public virtual async Task<Result<bool>> DeleteByKey<TEntity, TKey>(int IdUser, TKey key, bool filterCurrentUser = true) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<bool>() { Value = false };
            var userObjBase = await defaultRepository.GetByKey<TEntity, TKey>(key);
            if (userObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (userObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Value = await defaultRepository.DeleteByKey<TEntity, TKey>(key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to delete this entity.";
                        returnValue.Value = false;
                    }
                }
                else
                {
                    returnValue.Value = await defaultRepository.DeleteByKey<TEntity, TKey>(key);
                }
            }
            return returnValue;
        }
        public virtual async Task<Result<bool>> UpdateByKey<TEntity, TKey>(int IdUser, TEntity entity, TKey key, bool filterCurrentUser = true) where TEntity : DefaultEntity, new()
        {
            var returnValue = new Result<bool>() { Value = false };
            entity = entity.ConvertDatesToUtc();
            var entityObjBase = await defaultRepository.GetByKey<TEntity, TKey>(key);
            entity.UpdatedAt = DateTime.UtcNow;
            if (entityObjBase != null)
            {
                if (filterCurrentUser)
                {
                    if (entityObjBase.CreatorUserId == IdUser)
                    {
                        returnValue.Value = await defaultRepository.UpdateByKey(entity, key);
                    }
                    else
                    {
                        returnValue.Message = "User does not have permission to update this entity.";
                        returnValue.Value = false;
                    }
                }
                else
                {
                    returnValue.Value = await defaultRepository.UpdateByKey(entity, key);
                }
            }
            return returnValue;
        }
    }
}