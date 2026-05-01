using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Common;
using JffCsharpTools9.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Infra.Repositories
{
    [Obsolete("Use DefaultGuidRepository in JffCsharpTools9.Infra.Repositories instead for better separation of concerns and to avoid confusion with actual repository implementations.")]
    public class DefaultGuidRepository<T> : IDefaultGuidRepository<T> where T : DbContext
    {
        public Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByKey<TEntity, TKey>(TKey key, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] include = null, bool asNoTracking = false)
            where TEntity : DefaultGuidEntity<TEntity>, new()
            where TFilter : DefaultFilter<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByUser<TEntity>(Guid userId, string[] include = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<TEntity>> GetPaginated<TEntity>(PaginationResult<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false)
            where TEntity : DefaultGuidEntity<TEntity>, new()
            where TFilter : DefaultFilter<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<TEntity>> GetPaginatedByUser<TEntity>(PaginationResult<TEntity> pagination, Guid idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateBatch<TEntity>(IEnumerable<TEntity> entityList, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key, bool saveChanges = false) where TEntity : DefaultGuidEntity<TEntity>, new()
        {
            throw new NotImplementedException();
        }
    }
}