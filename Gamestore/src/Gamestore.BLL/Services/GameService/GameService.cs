using Gamestore.BLL.DTOs.Game;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.GameService;

public class GameService(
    IRepository<Game> repository,
    IRepository<GameGenre> gameGenreRepository,
    IRepository<GamePlatform> gamePlatformRepository,
    IRepository<Publisher> publisherRepository) : IGameService
{
    public async Task<ICollection<GameDto>> GetAllAsync()
    {
        var games = (await repository.GetAllAsync())
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<GameDto?> GetByKeyAsync(string key)
    {
        var game = await repository.GetOneAsync(g => g.Key == key);

        return game?.AsDto();
    }

    public async Task<ICollection<GameDto>> GetByGenreAsync(Guid genreId)
    {
        // get all gameIds by genre from GameGenre table
        var gameIds = (await gameGenreRepository
            .GetAllByFilterAsync(gg => gg.GenreId == genreId))
            .Select(gg => gg.GameId);

        // get all games from ids
        var games = (await repository
            .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<ICollection<GameDto>> GetByPlatformAsync(Guid platformId)
    {
        // get all gameIds by platform from GamePlatform table
        var gameIds = (await gamePlatformRepository
                .GetAllByFilterAsync(gg => gg.PlatformId == platformId))
            .Select(gg => gg.GameId);

        // get all games from ids
        var games = (await repository
                .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<ICollection<GameDto>> GetByPublisherAsync(string companyName)
    {
        var publisher = await publisherRepository.GetOneAsync(p => p.CompanyName == companyName)
                        ?? throw new PublisherNotFoundException(companyName);

        var games = (await repository
                .GetAllByFilterAsync(g => g.PublisherId == publisher.Id))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        var game = await repository.GetByIdAsync(id);

        return game?.AsDto();
    }

    public async Task UpdateAsync(UpdateGameDto dto)
    {
        var game = await repository.GetByIdAsync(dto.Id)
                   ?? throw new GameNotFoundException(dto.Id);
        dto.UpdateEntity(game);

        // delete old genres related to the game
        await gameGenreRepository.DeleteByFilterAsync(gg => gg.GameId == dto.Id);

        // delete old platforms related to the game
        await gamePlatformRepository.DeleteByFilterAsync(gp => gp.GameId == dto.Id);

        // add all genres
        if (dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = dto.Id, GenreId = genreId };
                await gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        // add all platforms
        if (dto.PlatformsIds.Count != 0)
        {
            foreach (var platformId in dto.PlatformsIds)
            {
                var gamePlatform = new GamePlatform { GameId = dto.Id, PlatformId = platformId };
                await gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGameDto dto)
    {
        var game = dto.AsEntity();

        await repository.CreateAsync(game);

        // add all genres
        if (dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
                await gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        // add all platforms
        if (dto.PlatformsIds.Count != 0)
        {
            foreach (var platformId in dto.PlatformsIds)
            {
                var gamePlatform = new GamePlatform { GameId = game.Id, PlatformId = platformId };
                await gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await repository.SaveChangesAsync();
    }

    public async Task DeleteByKeyAsync(string key)
    {
        await repository.DeleteByFilterAsync(g => g.Key == key);
        await repository.SaveChangesAsync();
    }
}