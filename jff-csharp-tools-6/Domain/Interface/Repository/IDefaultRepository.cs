using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools6.Domain.Interface.Repository
{
    public interface IDefaultRepository<T> where T : DbContext
    {
        Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> CreateInBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<TEntity> GetKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> filter, string[] include = null, bool asNoTracking = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> GetUser<TEntity>(int idUsuario, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<PaginationModel<TEntity>> GetPagination<TEntity>(PaginationModel<TEntity> paginacao, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> UpdateKey<TEntity, TKey>(TEntity entidade, TKey key) where TEntity : DefaultEntity<TEntity>, new();
        Task UpdateInBatch<TEntity>(IEnumerable<TEntity> listEntity, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> Delete<TEntity>(Expression<Func<TEntity, bool>> filter, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task DeleteInBatch<TEntity>(IEnumerable<TEntity> listEntity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> DeleteKey<TEntity, TKey>(TKey key) where TEntity : DefaultEntity<TEntity>, new();
        Task SaveChangesAsync();
    }
}