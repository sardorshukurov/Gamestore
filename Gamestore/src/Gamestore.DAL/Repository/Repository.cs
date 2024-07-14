using System.Linq.Expressions;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Repository;

public class Repository<T> : IRepository<T>
    where T : class
{
    private readonly MainDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(MainDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
        else
        {
            throw new NotFoundException($"Entity of type {typeof(T)} not found");
        }
    }

    public async Task DeleteByFilterAsync(Expression<Func<T, bool>> filter)
    {
        var entities = await _dbSet.Where(filter).ToListAsync();
        if (entities.Count != 0)
        {
            _dbSet.RemoveRange(entities);
        }
    }

    public async Task UpdateAsync(Guid id, T entity)
    {
        var existingEntity = await _dbSet.FindAsync(id);
        if (existingEntity is not null)
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            _dbSet.Update(existingEntity);
        }
        else
        {
            throw new NotFoundException($"Entity of type {typeof(T)} not found");
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.FirstOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.AsNoTracking()
            .Where(filter)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }
}