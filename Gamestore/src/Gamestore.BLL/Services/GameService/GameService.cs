using Gamestore.BLL.DTOs.Game;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.GameService;

public class GameService(IRepository<Game> repository, IRepository<GameGenre> gameGenreRepository, IRepository<GamePlatform> gamePlatformRepository) : IGameService
{
    private readonly IRepository<Game> _repository = repository;
    private readonly IRepository<GameGenre> _gameGenreRepository = gameGenreRepository;
    private readonly IRepository<GamePlatform> _gamePlatformRepository = gamePlatformRepository;

    public async Task<ICollection<GameDto>> GetAllAsync()
    {
        var games = (await _repository.GetAllAsync())
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<GameDto?> GetByKeyAsync(string key)
    {
        var game = await _repository.GetOneAsync(g => g.Key == key);

        return game?.AsDto();
    }

    public async Task<ICollection<GameDto>> GetByGenreAsync(Guid genreId)
    {
        var gameIds = (await _gameGenreRepository
            .GetAllByFilterAsync(gg => gg.GenreId == genreId))
            .Select(gg => gg.GameId);

        var games = (await _repository
            .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<ICollection<GameDto>> GetByPlatformAsync(Guid platformId)
    {
        var gameIds = (await _gamePlatformRepository
                .GetAllByFilterAsync(gg => gg.PlatformId == platformId))
            .Select(gg => gg.GameId);

        var games = (await _repository
                .GetAllByFilterAsync(g => gameIds.Contains(g.Id)))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        var game = await _repository.GetByIdAsync(id);

        return game?.AsDto();
    }

    public async Task UpdateAsync(Guid id, UpdateGameDto dto)
    {
        var game = await _repository.GetByIdAsync(id) ?? throw new Exception("Game not found");
        dto.UpdateEntity(game);

        await _gameGenreRepository.DeleteByFilterAsync(gg => gg.GameId == id);
        await _gamePlatformRepository.DeleteByFilterAsync(gp => gp.GameId == id);

        if (dto.GenresIds is not null && dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = id, GenreId = genreId };
                await _gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        if (dto.PlatformsIds is not null && dto.PlatformsIds.Count != 0)
        {
            foreach (var platformId in dto.PlatformsIds)
            {
                var gamePlatform = new GamePlatform { GameId = id, PlatformId = platformId };
                await _gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGameDto dto)
    {
        var game = dto.AsEntity();

        await _repository.CreateAsync(game);

        if (dto.GenresIds is not null && dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
                await _gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        if (dto.PlatformsIds is not null && dto.PlatformsIds.Count != 0)
        {
            foreach (var platformId in dto.PlatformsIds)
            {
                var gamePlatform = new GamePlatform { GameId = game.Id, PlatformId = platformId };
                await _gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteByKeyAsync(string key)
    {
        await _repository.DeleteByFilterAsync(g => g.Key == key);
        await _repository.SaveChangesAsync();
    }
}