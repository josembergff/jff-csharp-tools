using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
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

        public async Task<bool> UpdateKey<TEntity, TKey>(TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new()
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

        public virtual async Task UpdateInBatch<TEntity>(IEnumerable<TEntity> listEntity, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            if (forceDetach)
            {
                foreach (var entity in listEntity)
                {
                    Detach(entity);
                }
            }
            dbContext.UpdateRange(listEntity);
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

        public virtual async Task<IEnumerable<TEntity>> CreateInBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
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

        public async Task<TEntity> GetKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string lineInclue in include)
                    query = query.Include(lineInclue);

            var list = (DbSet<TEntity>)query;

            var atual = await list.FindAsync(key);

            return atual;
        }

        public async Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> listReturn = new List<TEntity>();

            if (pagination.Page > 0)
            {
                var query = dbContext.Set<TEntity>().Where(f => f.Id > 0);

                if (include != null && include.Any())
                    foreach (string includeLine in include)
                        query = query.Include(includeLine);

                pagination.Total = query.Count();
                if (pagination.CountPage > 0)
                {
                    query = query.Skip(pagination.CountPage * pagination.Page).Take(pagination.CountPage);
                }

                if (!string.IsNullOrEmpty(pagination.Order))
                {
                    var paramRefer = char.ToUpper(pagination.Order[0]) + pagination.Order.Substring(1);
                    var pi = typeof(TEntity).GetProperty(paramRefer);

                    if (pi != null && !string.IsNullOrEmpty(pagination.TypeOrder) && pagination.TypeOrder.ToUpper() == "DESC")
                    {
                        query = query.OrderByDescending(x => pi.GetValue(x, null));
                    }
                    else if (pi != null)
                    {
                        query = query.OrderBy(x => pi.GetValue(x, null));
                    }
                }
                listReturn = await query.ToListAsync();
            }
            else
            {
                listReturn = await dbContext.Set<TEntity>().Where(f => f.Id > 0).OrderByDescending(o => o.CreatedAt).ToListAsync();
                pagination.Total = listReturn.Count();
            }
            pagination.List = listReturn;
            return pagination;
        }

        public async Task<PaginationModel<TEntity>> GetPagination<TEntity>(PaginationModel<TEntity> paginacao, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> listReturn = new List<TEntity>();

            if (paginacao.Page > 0)
            {
                var query = dbContext.Set<TEntity>().Where(f => f.Id > 0);

                if (include != null && include.Any())
                    foreach (string lineInclue in include)
                        query = query.Include(lineInclue);

                paginacao.Total = query.Count();
                if (paginacao.CountPage > 0)
                {
                    query = query.Skip(paginacao.CountPage * paginacao.Page).Take(paginacao.CountPage);
                }

                if (!string.IsNullOrEmpty(paginacao.Order))
                {
                    var paramRefer = char.ToUpper(paginacao.Order[0]) + paginacao.Order.Substring(1);
                    var pi = typeof(TEntity).GetProperty(paramRefer);

                    if (pi != null && !string.IsNullOrEmpty(paginacao.TypeOrder) && paginacao.TypeOrder.ToUpper() == "DESC")
                    {
                        query = query.OrderByDescending(x => pi.GetValue(x, null));
                    }
                    else if (pi != null)
                    {
                        query = query.OrderBy(x => pi.GetValue(x, null));
                    }
                }
                listReturn = await query.ToListAsync();
            }
            else
            {
                listReturn = await dbContext.Set<TEntity>().Where(f => f.Id > 0).OrderByDescending(o => o.CreatedAt).ToListAsync();
                paginacao.Total = listReturn.Count();
            }
            paginacao.List = listReturn;
            return paginacao;
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

        public async Task<IEnumerable<TEntity>> GetUser<TEntity>(int idUsuario, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> lista = new List<TEntity>();
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string lineInclude in include)
                    query = query.Include(lineInclude);

            lista = await query.Where(f => f.Id > 0).OrderByDescending(o => o.CreatedAt).ToListAsync();

            return lista;
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

        public virtual async Task DeleteInBatch<TEntity>(IEnumerable<TEntity> listEntity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            dbContext.RemoveRange(listEntity);
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

        public async Task<bool> DeleteKey<TEntity, TKey>(TKey key) where TEntity : DefaultEntity<TEntity>, new()
        {
            var objBase = await dbContext.Set<TEntity>().FindAsync(key);
            if (objBase != null)
            {
                dbContext.Set<TEntity>().Remove(objBase);
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