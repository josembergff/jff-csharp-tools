using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Extensions;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Repository
{
    public class DefaultRepository<T> : IDefaultRepository<T> where T : DbContext
    {
        private readonly T dbContext;

        public DefaultRepository(T dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var existingEntity = await dbContext.Set<TEntity>().FindAsync(key);
            if (existingEntity != null)
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                dbContext.Update<TEntity>(entity);
            }
            return entity.Id > 0;
        }

        public virtual async Task UpdateBatch<TEntity>(IEnumerable<TEntity> entityList, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            if (forceDetach)
            {
                foreach (var entity in entityList)
                {
                    Detach(entity);
                }
            }
            dbContext.UpdateRange(entityList);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            await dbContext.Set<TEntity>().AddRangeAsync(entities);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return entities;
        }

        public async Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> list = new List<TEntity>();

            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            if (asNoTracking == true)
            {
                list = await query.Where(filter).OrderByDescending(o => o.CreatedAt).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await query.Where(filter).OrderByDescending(o => o.CreatedAt).ToListAsync();
            }

            return list;
        }

        public async Task<IEnumerable<TEntity>> GetByFilter<TEntity, TFilter>(TFilter filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            List<TEntity> list = new List<TEntity>();

            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            if (asNoTracking == true)
            {
                list = await query.Where(filter.Where()).OrderByDescending(o => o.CreatedAt).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await query.Where(filter.Where()).OrderByDescending(o => o.CreatedAt).ToListAsync();
            }

            return list;
        }

        public async Task<TEntity> GetByKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            var list = (DbSet<TEntity>)query;

            var current = await list.FindAsync(key);

            return current;
        }

        public virtual async Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();
            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);
            IQueryable<TEntity> items = query.Where(filter).ApplyOrderBy(pagination.OrderDescending, pagination.Order);

            if (!pagination.IgnorePagination)
            {
                items = query.Cast<TEntity>()
                        .ApplyOrderBy(!pagination.OrderDescending, pagination.Order)
                        .Skip(pagination.SkipTotal)
                        .Take(pagination.CountPerPage);
            }

            pagination.Total = await query.CountAsync();
            if (asNoTracking)
                pagination.List = await items.AsNoTracking().ToListAsync();
            else
                pagination.List = await items.ToListAsync();

            return pagination;
        }

        public virtual async Task<PaginationModel<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();
            var pagedList = new PaginationModel<TEntity>(filter);

            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);

            var baseQuery = query
                    .AsQueryable()
                    .Where(filter.Where());

            IQueryable<TEntity> items = baseQuery.ApplyOrderBy(filter.Asc, filter.OrderBy);

            if (!filter.IgnorePagination)
            {
                items = baseQuery.Cast<TEntity>()
                        .ApplyOrderBy(filter.Asc, filter.OrderBy)
                        .Skip(filter.SkipTotal)
                        .Take(filter.Count);
            }

            pagedList.Total = await baseQuery.CountAsync();
            if (asNoTracking)
                pagedList.List = await items.AsNoTracking().ToListAsync();
            else
                pagedList.List = await items.ToListAsync();

            return pagedList;
        }

        public async Task<PaginationModel<TEntity>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> pagination, int idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();
            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);
            IQueryable<TEntity> items = query.Where(f => f.CreatorUserId == idUser).ApplyOrderBy(pagination.OrderDescending, pagination.Order);

            if (!pagination.IgnorePagination)
            {
                items = query.Cast<TEntity>()
                        .ApplyOrderBy(!pagination.OrderDescending, pagination.Order)
                        .Skip(pagination.SkipTotal)
                        .Take(pagination.CountPerPage);
            }

            pagination.Total = await query.CountAsync();
            if (asNoTracking)
                pagination.List = await items.AsNoTracking().ToListAsync();
            else
                pagination.List = await items.ToListAsync();

            return pagination;
        }

        public async Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            var current = await query.FirstOrDefaultAsync(filter);

            return current;
        }

        public async Task<IEnumerable<TEntity>> GetByUser<TEntity>(int userId, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> list = new List<TEntity>();
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            list = await query.Where(f => f.Id > 0).OrderByDescending(o => o.CreatedAt).ToListAsync();

            return list;
        }

        public async Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            var baseList = await dbContext.Set<TEntity>().Where(filter).ToListAsync();
            if (baseList?.Any() == true)
            {
                dbContext.RemoveRange(baseList);
                if (saveChanges)
                {
                    await dbContext.SaveChangesAsync();
                }
            }
            return true;
        }

        public virtual async Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            dbContext.RemoveRange(entityList);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteByKey<TEntity, TKey>(TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var baseObj = await dbContext.Set<TEntity>().FindAsync(key);
            if (baseObj != null)
            {
                dbContext.Set<TEntity>().Remove(baseObj);
            }
            return true;
        }

        private void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity != null)
            {
                dbContext.Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}