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
    public async Task<ICollection<PlatformShortResponse>> GetAllAsync()
    {
        var platforms = (await repository.GetAllAsync())
            .Select(p => p.ToShortResponse())
            .ToList();

        return platforms;
    }

    public async Task<PlatformShortResponse?> GetByIdAsync(Guid id)
    {
        var platform = await repository.GetByIdAsync(id);

        return platform?.ToShortResponse();
    }

    public async Task UpdateAsync(UpdatePlatformRequest request)
    {
        var platformToUpdate = await repository.GetByIdAsync(request.Platform.Id)
                               ?? throw new PlatformNotFoundException(request.Platform.Id);

        request.UpdateEntity(platformToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreatePlatformRequest request)
    {
        var platformToAdd = request.ToEntity();

        await repository.CreateAsync(platformToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<ICollection<PlatformShortResponse>> GetAllByGameKeyAsync(string gameKey)
    {
        var game = (await gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                   throw new GameNotFoundException(gameKey);

        var platformIds = (await gamePlatformRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(p => p.PlatformId);

        var platforms = (await repository.GetAllByFilterAsync(p => platformIds.Contains(p.Id)))
            .Select(p => p.ToShortResponse())
            .ToList();

        return platforms;
    }
}