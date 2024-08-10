using System.Linq.Expressions;
using Gamestore.Domain.Common;
using MongoDB.Driver;

namespace Gamestore.DAL.Repository;

public class MongoRepository<T>(
    IMongoDatabase database,
    string collectionName) : IMongoRepository<T>
    where T : BaseMongoEntity
{
    private readonly IMongoCollection<T> _dbCollection = database.GetCollection<T>(collectionName);
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public async Task<ICollection<T>> GetAllAsync()
    {
        return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<ICollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbCollection.Find(filter).ToListAsync();
    }

    public async Task<T?> GetAsync(string id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await _dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(string id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        await _dbCollection.DeleteOneAsync(filter);
    }
}