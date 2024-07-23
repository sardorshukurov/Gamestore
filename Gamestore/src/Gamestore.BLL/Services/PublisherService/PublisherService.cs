using Gamestore.BLL.DTOs.Publisher;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.PublisherService;

public class PublisherService(
    IRepository<Publisher> repository,
    IRepository<Game> gameRepository) : IPublisherService
{
    public async Task<ICollection<PublisherDto>> GetAllAsync()
    {
        var publishers = (await repository.GetAllAsync())
            .Select(p => p.ToDto())
            .ToList();

        return publishers;
    }

    public async Task<PublisherDto?> GetByIdAsync(Guid id)
    {
        var publisher = await repository.GetByIdAsync(id);

        return publisher?.ToDto();
    }

    public async Task UpdateAsync(UpdatePublisherDto dto)
    {
        var publisherToUpdate = await repository.GetByIdAsync(dto.Id)
                                ?? throw new PublisherNotFoundException(dto.Id);

        dto.UpdateEntity(publisherToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePublisherDto dto)
    {
        var publisherToAdd = dto.ToEntity();

        await repository.CreateAsync(publisherToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<PublisherDto?> GetByCompanyNameAsync(string companyName)
    {
        var publisher = await repository
            .GetOneAsync(p => p.CompanyName == companyName);

        return publisher?.ToDto();
    }

    public async Task<PublisherDto?> GetByGameKeyAsync(string gameKey)
    {
        var game = await gameRepository.GetOneAsync(g => g.Key == gameKey) ?? throw new GameNotFoundException(gameKey);

        var publisher = await repository.GetByIdAsync(game.PublisherId);

        return publisher?.ToDto();
    }
}