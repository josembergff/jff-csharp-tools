using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Model;

namespace JffCsharpTools.Domain.Interface.Repository
{
    public interface IDefaultRepository
    {
        Task<TEntity> Create<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> CreateInBatch<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<TEntity> GetKey<TEntity, TKey>(TKey key, string[] include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<TEntity> GetFirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filtro, string[]? include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> Obter<TEntity>(Expression<Func<TEntity, bool>> filtro, string[]? include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<IEnumerable<TEntity>> ObterUsuario<TEntity>(int idUsuario, string[]? include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<PaginationModel<TEntity>> ObterPaginacao<TEntity>(PaginationModel<TEntity> paginacao, string[]? include = null) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> UpdateKey<TEntity, TKey>(TEntity entidade, TKey key) where TEntity : DefaultEntity<TEntity>, new();
        Task AtualizarEmLote<TEntity>(IEnumerable<TEntity> listEntity, bool forceDetach = false, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> Remover<TEntity>(Expression<Func<TEntity, bool>> filtro, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task RemoverEmLote<TEntity>(IEnumerable<TEntity> listEntity, bool saveChanges = false) where TEntity : DefaultEntity<TEntity>, new();
        Task<bool> DeleteKey<TEntity, TKey>(TKey key) where TEntity : DefaultEntity<TEntity>, new();
    }
}