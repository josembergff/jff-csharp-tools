using System.Collections.Generic;
using System.Threading.Tasks;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Interface.Repository;
using JffCsharpTools.Domain.Model;

namespace JffCsharpTools.Domain.Interface.Service
{
    public interface IDefaultService
    {
        IDefaultRepository defaultRepository { get; set; }
        Task<DefaultResponseModel<int>> Create<TEntity>(int IdUser, TEntity entity) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<TEntity>> GetKey<TEntity, Tkey>(int IdUser, Tkey key, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<IEnumerable<TEntity>>> Get<TEntity>(int IdUser, TEntity entityFilter = null, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<PaginationModel<TEntity>>> GetPagination<TEntity>(int IdUser, PaginationModel<TEntity> pagination, string[] includes = null) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<bool>> Update<TEntity, TKey>(int IdUser, TEntity entity, TKey key) where TEntity : DefaultEntity<TEntity>, new();

        Task<DefaultResponseModel<bool>> DeleteKey<TEntity, TKey>(int IdUser, TKey key) where TEntity : DefaultEntity<TEntity>, new();
    }

}