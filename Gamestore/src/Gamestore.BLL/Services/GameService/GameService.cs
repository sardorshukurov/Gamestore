using Gamestore.BLL.DTOs.Game;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Filtration.Games;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.GameService;

public class GameService(
    IRepository<Game> repository,
    IGamesFilterRepository filterRepository,
    IRepository<GameGenre> gameGenreRepository,
    IRepository<GamePlatform> gamePlatformRepository,
    IRepository<Publisher> publisherRepository) : IGameService
{
    public async Task<GamesResponse> GetAllAsync(SearchCriteria criteria)
    {
        var filteredGames = (await filterRepository.GetAsync(criteria))
            .Select(g => g.ToResponse())
            .ToList();

        int totalItems = filteredGames.Count;
        int totalPages = criteria.PageCount == 0 ? 1 : (int)Math.Ceiling(totalItems / (double)criteria.PageCount);
        return new GamesResponse(filteredGames, totalPages, criteria.Page);
    }

    public async Task<GameResponse?> GetByKeyAsync(string key)
    {
        var game = await repository.GetOneAsync(g => g.Key == key);

        return game?.ToResponse();
    }

    public async Task<ICollection<GameResponse>> GetByGenreAsync(Guid genreId)
    {
        // get all gameIds by genre from GameGenre table
        var gameIds = (await gameGenreRepository
            .GetAllByFilterAsync(gg => gg.GenreId == genreId))
            .Select(gg => gg.GameId);

        // get all games from ids
        var games = (await repository
            .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.ToResponse())
            .ToList();

        return games;
    }

    public async Task<ICollection<GameResponse>> GetByPlatformAsync(Guid platformId)
    {
        // get all gameIds by platform from GamePlatform table
        var gameIds = (await gamePlatformRepository
                .GetAllByFilterAsync(gg => gg.PlatformId == platformId))
            .Select(gg => gg.GameId);

        // get all games from ids
        var games = (await repository
                .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.ToResponse())
            .ToList();

        return games;
    }

    public async Task<ICollection<GameResponse>> GetByPublisherAsync(string companyName)
    {
        var publisher = await publisherRepository.GetOneAsync(p => p.CompanyName == companyName)
                        ?? throw new PublisherNotFoundException(companyName);

        var games = (await repository
                .GetAllByFilterAsync(g => g.PublisherId == publisher.Id))
            .Select(g => g.ToResponse())
            .ToList();

        return games;
    }

    public async Task<GameResponse?> GetByIdAsync(Guid id)
    {
        var game = await repository.GetByIdAsync(id);

        return game?.ToResponse();
    }

    public async Task UpdateAsync(UpdateGameRequest request)
    {
        var game = await repository.GetByIdAsync(request.Game.Id)
                   ?? throw new GameNotFoundException(request.Game.Id);
        request.UpdateEntity(game);

        // delete old genres related to the game
        await gameGenreRepository.DeleteByFilterAsync(gg => gg.GameId == request.Game.Id);

        // delete old platforms related to the game
        await gamePlatformRepository.DeleteByFilterAsync(gp => gp.GameId == request.Game.Id);

        // add all genres
        if (request.Genres.Any())
        {
            foreach (var genreId in request.Genres)
            {
                var gameGenre = new GameGenre { GameId = request.Game.Id, GenreId = genreId };
                await gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        // add all platforms
        if (request.Platforms.Any())
        {
            foreach (var platformId in request.Platforms)
            {
                var gamePlatform = new GamePlatform { GameId = request.Game.Id, PlatformId = platformId };
                await gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGameRequest request)
    {
        var game = request.ToEntity();

        await repository.CreateAsync(game);

        // add all genres
        if (request.Genres.Any())
        {
            foreach (var genreId in request.Genres)
            {
                var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
                await gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        // add all platforms
        if (request.Platforms.Any())
        {
            foreach (var platformId in request.Platforms)
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