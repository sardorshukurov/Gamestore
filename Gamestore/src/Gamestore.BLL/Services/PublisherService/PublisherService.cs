using Gamestore.BLL.DTOs.Publisher;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.PublisherService;

public class PublisherService(IRepository<Publisher> repository, IRepository<Game> gameRepository) : IPublisherService
{
    private readonly IRepository<Publisher> _repository = repository;
    private readonly IRepository<Game> _gameRepository = gameRepository;

    public async Task<ICollection<PublisherDto>> GetAllAsync()
    {
        var publishers = (await _repository.GetAllAsync())
            .Select(p => p.AsDto())
            .ToList();

        return publishers;
    }

    public async Task<PublisherDto?> GetByIdAsync(Guid id)
    {
        var publisher = await _repository.GetByIdAsync(id);

        return publisher?.AsDto();
    }

    public async Task UpdateAsync(UpdatePublisherDto dto)
    {
        var publisherToUpdate = await _repository.GetByIdAsync(dto.Id)
                                ?? throw new PublisherNotFoundException(dto.Id);

        dto.UpdateEntity(publisherToUpdate);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePublisherDto dto)
    {
        var publisherToAdd = dto.AsEntity();

        await _repository.CreateAsync(publisherToAdd);
        await _repository.SaveChangesAsync();
    }

    public async Task<ICollection<PublisherDto>> GetByCompanyNameAsync(string companyName)
    {
        var publishers = (await _repository
            .GetAllByFilterAsync(p => p.CompanyName == companyName))
            .Select(p => p.AsDto())
            .ToList();

        return publishers;
    }

    public async Task<PublisherDto?> GetAllByGameKeyAsync(string gameKey)
    {
        var game = await _gameRepository.GetOneAsync(g => g.Key == gameKey) ?? throw new GameNotFoundException(gameKey);

        var publisher = await _repository.GetByIdAsync(game.PublisherId);

        return publisher?.AsDto();
    }
}