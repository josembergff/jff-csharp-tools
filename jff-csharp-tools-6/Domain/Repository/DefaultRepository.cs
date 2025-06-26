using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Extensions;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using JffCsharpTools6.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Domain.Repository
{
    public class DefaultRepository<T> : IDefaultRepository<T> where T : DbContext
    {
        private readonly T dbContext;

        public DefaultRepository(T dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Update an entity by its key. If the entity does not exist, it will be added.
        /// If the entity exists, its current values will be updated with the provided entity values.
        /// If saveChanges is true, changes will be saved to the database.
        /// If the entity has an Id greater than 0, it is considered updated.
        /// If the entity does not exist, it will be added to the context.
        /// 
        /// This method is useful for updating an entity by its key, ensuring that the entity is either updated or created as needed.
        /// 
        /// Example usage:
        /// var updated = await repository.UpdateByKey(entity, key, true);
        /// 
        /// Returns true if the entity was updated or created successfully.
        /// If the entity was not found and added, it will return false.
        /// 
        /// Note: This method assumes that the entity has a property named Id of type int or long.
        /// If the entity does not have an Id property, you may need to modify the method accordingly.
        /// 
        /// This method is generic and can be used with any entity type that inherits from DefaultEntity<TEntity>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        public async Task<bool> UpdateByKey<TEntity, TKey>(TEntity entity, TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
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
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return entity.Id > 0;
        }

        /// <summary>
        /// Updates a batch of entities.
        /// If forceDetach is true, entities will be detached before updating.
        /// If saveChanges is true, changes will be saved to the database.
        /// 
        /// Example:
        /// await repository.UpdateBatch(entityList, true, true);
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entityList">List of entities to update.</param>
        /// <param name="forceDetach">If true, detaches entities before updating.</param>
        /// <param name="saveChanges">If true, saves changes to the database.</param>
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

        /// <summary>
        /// Cria uma nova entidade no banco de dados.
        /// Se saveChanges for true, as alterações serão salvas imediatamente.
        /// 
        /// Exemplo de uso:
        /// var novaEntidade = await repository.Create(entidade, true);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="entity">Entidade a ser criada.</param>
        /// <param name="saveChanges">Se true, salva as alterações no banco de dados.</param>
        /// <returns>Entidade criada.</returns>
        public async Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return entity;
        }

        /// <summary>
        /// Creates multiple entities in the database.
        /// If saveChanges is true, changes will be saved immediately.
        /// 
        /// Example:
        /// var createdEntities = await repository.CreateBatch(entityList, true);
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entities">Collection of entities to create.</param>
        /// <param name="saveChanges">If true, saves changes to the database.</param>
        /// <returns>List of created entities.</returns>
        public virtual async Task<IEnumerable<TEntity>> CreateBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            await dbContext.Set<TEntity>().AddRangeAsync(entities);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return entities;
        }

        /// <summary>
        /// Obtém entidades que atendem ao filtro especificado.
        /// Permite incluir propriedades de navegação e usar AsNoTracking.
        /// 
        /// Exemplo de uso:
        /// var lista = await repository.Get(x => x.Ativo, new[] { "Propriedade" }, true);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="filter">Expressão lambda para filtrar as entidades.</param>
        /// <param name="include">Propriedades de navegação a serem incluídas.</param>
        /// <param name="asNoTracking">Se true, consulta sem rastreamento.</param>
        /// <returns>Lista de entidades encontradas.</returns>
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

        /// <summary>
        /// Obtém entidades usando um filtro customizado (DefaultFilter).
        /// Permite incluir propriedades de navegação e usar AsNoTracking.
        /// 
        /// Exemplo de uso:
        /// var lista = await repository.GetByFilter<MyEntity, MyFilter>(filtro, new[] { "Propriedade" }, true);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <typeparam name="TFilter">Tipo do filtro customizado.</typeparam>
        /// <param name="filter">Filtro customizado.</param>
        /// <param name="include">Propriedades de navegação a serem incluídas.</param>
        /// <param name="asNoTracking">Se true, consulta sem rastreamento.</param>
        /// <returns>Lista de entidades encontradas.</returns>
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

        /// <summary>
        /// Obtém uma entidade pelo valor da chave primária.
        /// Permite incluir propriedades de navegação.
        /// 
        /// Exemplo de uso:
        /// var entidade = await repository.GetByKey<MyEntity, int>(id, new[] { "Propriedade" });
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <typeparam name="TKey">Tipo da chave primária.</typeparam>
        /// <param name="key">Valor da chave primária.</param>
        /// <param name="include">Propriedades de navegação a serem incluídas.</param>
        /// <returns>Entidade encontrada ou null.</returns>
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

        /// <summary>
        /// Obtém uma lista paginada de entidades com base em um filtro e paginação.
        /// Permite incluir propriedades de navegação e usar AsNoTracking.
        /// 
        /// Exemplo de uso:
        /// var paginacao = await repository.GetPaginated(paginationModel, x => x.Ativo);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="pagination">Modelo de paginação.</param>
        /// <param name="filter">Expressão lambda para filtrar as entidades.</param>
        /// <param name="includes">Propriedades de navegação a serem incluídas.</param>
        /// <param name="asNoTracking">Se true, consulta sem rastreamento.</param>
        /// <returns>Modelo de paginação preenchido.</returns>
        public virtual async Task<PaginationModel<TEntity>> GetPaginated<TEntity>(PaginationModel<TEntity> pagination, Expression<Func<TEntity, bool>> filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>().Where(filter);
            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);
            IQueryable<TEntity> items = query.ApplyOrderBy(pagination.OrderDescending, pagination.Order);

            if (!pagination.IgnorePagination)
            {
                items = query.Cast<TEntity>()
                        .Where(filter)
                        .ApplyOrderBy(pagination.OrderDescending, pagination.Order)
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

        /// <summary>
        /// Obtém uma lista paginada de entidades usando um filtro customizado (DefaultFilter).
        /// Permite incluir propriedades de navegação e usar AsNoTracking.
        /// 
        /// Exemplo de uso:
        /// var paginacao = await repository.GetPaginatedByFilter<MyEntity, MyFilter>(filtro);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <typeparam name="TFilter">Tipo do filtro customizado.</typeparam>
        /// <param name="filter">Filtro customizado.</param>
        /// <param name="includes">Propriedades de navegação a serem incluídas.</param>
        /// <param name="asNoTracking">Se true, consulta sem rastreamento.</param>
        /// <returns>Modelo de paginação preenchido.</returns>
        public virtual async Task<PaginationModel<TEntity>> GetPaginatedByFilter<TEntity, TFilter>(TFilter filter, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new() where TFilter : DefaultFilter<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>().Where(filter.Where());
            var pagedList = new PaginationModel<TEntity>(filter);

            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);

            IQueryable<TEntity> items = query.ApplyOrderBy(filter.Asc, filter.OrderBy);

            if (!filter.IgnorePagination)
            {
                items = query.Cast<TEntity>()
                        .ApplyOrderBy(filter.Asc, filter.OrderBy)
                        .Skip(filter.SkipTotal)
                        .Take(filter.Count);
            }

            pagedList.Total = await query.CountAsync();
            if (asNoTracking)
                pagedList.List = await items.AsNoTracking().ToListAsync();
            else
                pagedList.List = await items.ToListAsync();

            return pagedList;
        }

        /// <summary>
        /// Obtém uma lista paginada de entidades criadas por um usuário específico.
        /// Permite incluir propriedades de navegação e usar AsNoTracking.
        /// 
        /// Exemplo de uso:
        /// var paginacao = await repository.GetPaginatedByUser<MyEntity>(paginationModel, idUsuario);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="pagination">Modelo de paginação.</param>
        /// <param name="idUser">Id do usuário criador.</param>
        /// <param name="includes">Propriedades de navegação a serem incluídas.</param>
        /// <param name="asNoTracking">Se true, consulta sem rastreamento.</param>
        /// <returns>Modelo de paginação preenchido.</returns>
        public async Task<PaginationModel<TEntity>> GetPaginatedByUser<TEntity>(PaginationModel<TEntity> pagination, int idUser, string[] includes = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>().Where(f => f.CreatorUserId == idUser);
            if (includes != null && includes.Any())
                foreach (string include in includes)
                    query = query.Include(include);
            IQueryable<TEntity> items = query.ApplyOrderBy(pagination.OrderDescending, pagination.Order);

            if (!pagination.IgnorePagination)
            {
                items = query.Cast<TEntity>()
                        .ApplyOrderBy(pagination.OrderDescending, pagination.Order)
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

        /// <summary>
        /// Obtém a primeira entidade que atende ao filtro especificado.
        /// Permite incluir propriedades de navegação.
        /// 
        /// Exemplo de uso:
        /// var entidade = await repository.GetFirstOrDefault<MyEntity>(x => x.Ativo);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="filter">Expressão lambda para filtrar as entidades.</param>
        /// <param name="include">Propriedades de navegação a serem incluídas.</param>
        /// <returns>Primeira entidade encontrada ou null.</returns>
        public async Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            var current = await query.FirstOrDefaultAsync(filter);

            return current;
        }

        /// <summary>
        /// Obtém todas as entidades criadas por um usuário específico.
        /// Permite incluir propriedades de navegação.
        /// 
        /// Exemplo de uso:
        /// var lista = await repository.GetByUser<MyEntity>(idUsuario);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="userId">Id do usuário criador.</param>
        /// <param name="include">Propriedades de navegação a serem incluídas.</param>
        /// <returns>Lista de entidades encontradas.</returns>
        public async Task<IEnumerable<TEntity>> GetByUser<TEntity>(int userId, string[] include = null) where TEntity : DefaultEntity<TEntity>, new()
        {
            List<TEntity> list = new List<TEntity>();
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (include?.Any() == true)
                foreach (string includeLine in include)
                    query = query.Include(includeLine);

            list = await query.Where(f => f.CreatorUserId == userId).OrderByDescending(o => o.CreatedAt).ToListAsync();

            return list;
        }

        /// <summary>
        /// Remove entidades que atendem ao filtro especificado.
        /// Se saveChanges for true, as alterações serão salvas imediatamente.
        /// 
        /// Exemplo de uso:
        /// var sucesso = await repository.Delete<MyEntity>(x => x.Ativo == false, true);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="filter">Expressão lambda para filtrar as entidades a serem removidas.</param>
        /// <param name="saveChanges">Se true, salva as alterações no banco de dados.</param>
        /// <returns>True se a operação foi realizada.</returns>
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

        /// <summary>
        /// Deletes a batch of entities.
        /// If saveChanges is true, changes will be saved immediately.
        /// 
        /// Example:
        /// await repository.DeleteBatch(entityList, true);
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entityList">List of entities to delete.</param>
        /// <param name="saveChanges">If true, saves changes to the database.</param>
        public virtual async Task DeleteBatch<TEntity>(IEnumerable<TEntity> entityList, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            dbContext.RemoveRange(entityList);
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes an entity by its primary key value.
        /// If saveChanges is true, changes will be saved immediately.
        /// 
        /// Example:
        /// var success = await repository.DeleteByKey<MyEntity, int>(id, true);
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <param name="key">Primary key value.</param>
        /// <param name="saveChanges">If true, saves changes to the database.</param>
        /// <returns>True if the operation was performed.</returns>
        public async Task<bool> DeleteByKey<TEntity, TKey>(TKey key, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new()
        {
            var baseObj = await dbContext.Set<TEntity>().FindAsync(key);
            if (baseObj != null)
            {
                dbContext.Set<TEntity>().Remove(baseObj);
            }
            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
            return true;
        }
        /// <summary>
        /// Desanexa uma entidade do DbContext.
        /// 
        /// Exemplo de uso:
        /// Detach(entidade);
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade.</typeparam>
        /// <param name="entity">Entidade a ser desanexada.</param>
        private void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity != null)
            {
                dbContext.Entry(entity).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Salva as alterações no banco de dados de forma assíncrona.
        /// 
        /// Exemplo de uso:
        /// await repository.SaveChangesAsync();
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Realiza o rollback da transação atual e limpa o ChangeTracker.
        /// 
        /// Exemplo de uso:
        /// await repository.Rollback();
        /// </summary>
        public async Task Rollback()
        {
            if (dbContext.Database.CurrentTransaction != null)
            {
                await dbContext.Database.RollbackTransactionAsync();
            }
            dbContext.ChangeTracker.Clear();
        }
    }
}