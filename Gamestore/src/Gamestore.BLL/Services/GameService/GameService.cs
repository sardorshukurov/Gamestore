using Gamestore.BLL.DTOs.Game;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.GameService;

public class GameService(IRepository<Game> repository, IRepository<GameGenre> gameGenreRepository, IRepository<GamePlatform> gamePlatformRepository, IRepository<Publisher> publisherRepository) : IGameService
{
    private readonly IRepository<Game> _repository = repository;
    private readonly IRepository<GameGenre> _gameGenreRepository = gameGenreRepository;
    private readonly IRepository<GamePlatform> _gamePlatformRepository = gamePlatformRepository;
    private readonly IRepository<Publisher> _publisherRepository = publisherRepository;

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

    public async Task<ICollection<GameDto>> GetByPublisherAsync(string companyName)
    {
        var publisher = await _publisherRepository.GetOneAsync(p => p.CompanyName == companyName) ?? throw new PublisherNotFoundException(companyName);
        var games = (await _repository
                .GetAllByFilterAsync(g => g.PublisherId == publisher.Id))
            .Select(g => g.AsDto())
            .ToList();

        return games;
    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        var game = await _repository.GetByIdAsync(id);

        return game?.AsDto();
    }

    public async Task UpdateAsync(UpdateGameDto dto)
    {
        var game = await _repository.GetByIdAsync(dto.Id) ?? throw new GameNotFoundException(dto.Id);
        dto.UpdateEntity(game);

        await _gameGenreRepository.DeleteByFilterAsync(gg => gg.GameId == dto.Id);
        await _gamePlatformRepository.DeleteByFilterAsync(gp => gp.GameId == dto.Id);

        if (dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = dto.Id, GenreId = genreId };
                await _gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        if (dto.PlatformsIds.Count != 0)
        {
            foreach (var platformId in dto.PlatformsIds)
            {
                var gamePlatform = new GamePlatform { GameId = dto.Id, PlatformId = platformId };
                await _gamePlatformRepository.CreateAsync(gamePlatform);
            }
        }

        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGameDto dto)
    {
        var game = dto.AsEntity();

        await _repository.CreateAsync(game);

        if (dto.GenresIds.Count != 0)
        {
            foreach (var genreId in dto.GenresIds)
            {
                var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
                await _gameGenreRepository.CreateAsync(gameGenre);
            }
        }

        if (dto.PlatformsIds.Count != 0)
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