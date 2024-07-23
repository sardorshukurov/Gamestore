using Gamestore.BLL.DTOs.Publisher;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.PublisherService;

public class PublisherService(
    IRepository<Publisher> repository,
    IRepository<Game> gameRepository) : IPublisherService
{
    public async Task<ICollection<PublisherResponse>> GetAllAsync()
    {
        var publishers = (await repository.GetAllAsync())
            .Select(p => p.ToResponse())
            .ToList();

        return publishers;
    }

    public async Task<PublisherResponse?> GetByIdAsync(Guid id)
    {
        var publisher = await repository.GetByIdAsync(id);

        return publisher?.ToResponse();
    }

    public async Task UpdateAsync(UpdatePublisherRequest request)
    {
        var publisherToUpdate = await repository.GetByIdAsync(request.Publisher.Id)
                                ?? throw new PublisherNotFoundException(request.Publisher.Id);

        request.UpdateEntity(publisherToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePublisherRequest request)
    {
        var publisherToAdd = request.ToEntity();

        await repository.CreateAsync(publisherToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<PublisherResponse?> GetByCompanyNameAsync(string companyName)
    {
        var publisher = await repository
            .GetOneAsync(p => p.CompanyName == companyName);

        return publisher?.ToResponse();
    }

    public async Task<PublisherResponse?> GetByGameKeyAsync(string gameKey)
    {
        var game = await gameRepository.GetOneAsync(g => g.Key == gameKey) ?? throw new GameNotFoundException(gameKey);

        var publisher = await repository.GetByIdAsync(game.PublisherId);

        return publisher?.ToResponse();
    }
}