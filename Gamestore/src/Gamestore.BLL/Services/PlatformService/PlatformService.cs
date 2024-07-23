using Gamestore.BLL.DTOs.Platform;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.PlatformService;

public class PlatformService(
    IRepository<Platform> repository,
    IRepository<Game> gameRepository,
    IRepository<GamePlatform> gamePlatformRepository) : IPlatformService
{
    public async Task<ICollection<PlatformShortDto>> GetAllAsync()
    {
        var platforms = (await repository.GetAllAsync())
            .Select(p => p.ToShortDto())
            .ToList();

        return platforms;
    }

    public async Task<PlatformShortDto?> GetByIdAsync(Guid id)
    {
        var platform = await repository.GetByIdAsync(id);

        return platform?.ToShortDto();
    }

    public async Task UpdateAsync(UpdatePlatformDto dto)
    {
        var platformToUpdate = await repository.GetByIdAsync(dto.Id)
                               ?? throw new PlatformNotFoundException(dto.Id);

        dto.UpdateEntity(platformToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePlatformDto dto)
    {
        var platformToAdd = dto.ToEntity();

        await repository.CreateAsync(platformToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<ICollection<PlatformShortDto>> GetAllByGameKeyAsync(string gameKey)
    {
        var game = (await gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                   throw new GameNotFoundException(gameKey);

        var platformIds = (await gamePlatformRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(p => p.PlatformId);

        var platforms = (await repository.GetAllByFilterAsync(p => platformIds.Contains(p.Id)))
            .Select(p => p.ToShortDto())
            .ToList();

        return platforms;
    }
}