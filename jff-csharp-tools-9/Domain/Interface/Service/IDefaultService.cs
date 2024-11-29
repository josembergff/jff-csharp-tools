using System.Collections.Generic;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Model;
using JffCsharpTools9.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace JffCsharpTools9.Domain.Interface.Service
{
    public interface IDefaultService<T> where T : DbContext
    {
        IDefaultRepository<T> defaultRepository { get; set; }
        Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<TEntity>> GetKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPagination<TEntity>(int IdUser, PaginationModel<TEntity> pagination, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<bool>> Update<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<bool>> DeleteKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new();
    }

}