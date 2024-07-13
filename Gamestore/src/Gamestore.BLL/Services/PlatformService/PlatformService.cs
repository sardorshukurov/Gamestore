using Gamestore.BLL.DTOs.Platform;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.PlatformService;

public class PlatformService(IRepository<Platform> repository, IRepository<Game> gameRepository, IRepository<GamePlatform> gamePlatformRepository) : IPlatformService
{
    private readonly IRepository<Platform> _repository = repository;
    private readonly IRepository<Game> _gameRepository = gameRepository;
    private readonly IRepository<GamePlatform> _gamePlatformRepository = gamePlatformRepository;

    public async Task<ICollection<PlatformShortDto>> GetAllAsync()
    {
        var platforms = (await _repository.GetAllAsync())
            .Select(p => p.AsShortDto())
            .ToList();

        return platforms;
    }

    public async Task<PlatformShortDto?> GetByIdAsync(Guid id)
    {
        var platform = await _repository.GetByIdAsync(id);

        return platform?.AsShortDto();
    }

    public async Task UpdateAsync(UpdatePlatformDto dto)
    {
        // TODO: Replace with custom exception
        var platformToUpdate = await _repository.GetByIdAsync(dto.Id) ?? throw new Exception("Platform not found");

        dto.UpdateEntity(platformToUpdate);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePlatformDto dto)
    {
        var platformToAdd = dto.AsEntity();

        await _repository.CreateAsync(platformToAdd);
        await _repository.SaveChangesAsync();
    }

    public async Task<ICollection<PlatformShortDto>> GetAllByGameKeyAsync(string gameKey)
    {
        // TODO: replace with custom exception
        var game = (await _gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                   throw new Exception("Game not found");

        var platformIds = (await _gamePlatformRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(p => p.PlatformId);

        var platforms = (await _repository.GetAllByFilterAsync(p => platformIds.Contains(p.Id)))
            .Select(p => p.AsShortDto())
            .ToList();

        return platforms;
    }
}