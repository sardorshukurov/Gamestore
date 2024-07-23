using System.Linq.Expressions;

namespace Gamestore.DAL.Repository;

public interface IRepository<T>
{
    Task CreateAsync(T entity);

    Task DeleteByIdAsync(Guid id);

    Task DeleteOneAsync(Expression<Func<T, bool>> filter);

    Task DeleteByFilterAsync(Expression<Func<T, bool>> filter);

    Task UpdateAsync(Guid id, T entity);

    Task<T?> GetByIdAsync(Guid id);

    Task<T?> GetOneAsync(Expression<Func<T, bool>> filter);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter);

    Task SaveChangesAsync();

    Task<int> CountAsync();

    Task<int> CountByFilterAsync(Expression<Func<T, bool>> filter);

    Task<bool> Exists(Expression<Func<T, bool>> filter);
}