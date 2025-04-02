using InfrastructorLayer.Model;
using System.Linq.Expressions;

namespace InfrastructorLayer.DataAccess.Contracts
{
    public interface  IStorageRepository<TEntity, TId>
        where TEntity:BaseRecord<TId>
        
    {
        Task<List<TEntity>> GetAllAsync(params string[] includeList);
        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity,bool>> predicate,params string[] includeList);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeList);
        Task<TEntity> GetByIdAsync(TId id,params string[] includeList);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
