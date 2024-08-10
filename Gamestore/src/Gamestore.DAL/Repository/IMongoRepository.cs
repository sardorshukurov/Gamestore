using System.Linq.Expressions;
using Gamestore.Domain.Common;

namespace Gamestore.DAL.Repository;

public interface IMongoRepository<T>
    where T : BaseMongoEntity
{
    Task CreateAsync(T entity);

    Task<ICollection<T>> GetAllAsync();

    Task<ICollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter);

    Task<T?> GetAsync(string id);

    Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter);

    Task UpdateAsync(T entity);

    Task RemoveAsync(string id);
}