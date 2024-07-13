using Gamestore.BLL.DTOs.Genre;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.GenreService;

public class GenreService(IRepository<Genre> repository, IRepository<GameGenre> gameGenreRepository, IRepository<Game> gameRepository) : IGenreService
{
    private readonly IRepository<Genre> _repository = repository;
    private readonly IRepository<GameGenre> _gameGenreRepository = gameGenreRepository;
    private readonly IRepository<Game> _gameRepository = gameRepository;

    public async Task<ICollection<GenreShortDto>> GetAllAsync()
    {
        var genres = (await _repository.GetAllAsync())
            .Select(g => g.AsShortDto())
            .ToList();

        return genres;
    }

    public async Task<GenreShortDto?> GetByIdAsync(Guid id)
    {
        var genre = await _repository.GetByIdAsync(id);

        return genre?.AsShortDto();
    }

    public async Task<ICollection<GenreShortDto>> GetSubGenresAsync(Guid parentId)
    {
        var subGenres = (await _repository.GetAllByFilterAsync(g => g.ParentGenreId == parentId))
            .Select(g => g.AsShortDto())
            .ToList();

        return subGenres;
    }

    public async Task UpdateAsync(UpdateGenreDto dto)
    {
        if (dto.ParentGenreId is not null)
        {
            _ = await _repository.GetByIdAsync((Guid)dto.ParentGenreId) ?? throw new GenreNotFoundException((Guid)dto.ParentGenreId);
        }

        var genreToUpdate = await _repository.GetByIdAsync(dto.Id)
                            ?? throw new GenreNotFoundException(dto.Id);

        dto.UpdateEntity(genreToUpdate);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGenreDto dto)
    {
        if (dto.ParentGenreId is not null)
        {
            _ = await _repository.GetByIdAsync((Guid)dto.ParentGenreId) ?? throw new GenreNotFoundException((Guid)dto.ParentGenreId);
        }

        var genreToAdd = dto.AsEntity();

        await _repository.CreateAsync(genreToAdd);
        await _repository.SaveChangesAsync();
    }

    public async Task<ICollection<GenreShortDto>> GetAllByGameKeyAsync(string gameKey)
    {
        var game = (await _gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                     throw new GameNotFoundException(gameKey);

        var genreIds = (await _gameGenreRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(g => g.GenreId);

        var genres = (await _repository.GetAllByFilterAsync(g => genreIds.Contains(g.Id)))
            .Select(g => g.AsShortDto())
            .ToList();

        return genres;
    }
}