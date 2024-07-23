using Gamestore.BLL.DTOs.Genre;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.GenreService;

public class GenreService(
    IRepository<Genre> repository,
    IRepository<GameGenre> gameGenreRepository,
    IRepository<Game> gameRepository) : IGenreService
{
    public async Task<ICollection<GenreShortDto>> GetAllAsync()
    {
        var genres = (await repository.GetAllAsync())
            .Select(g => g.ToShortDto())
            .ToList();

        return genres;
    }

    public async Task<GenreShortDto?> GetByIdAsync(Guid id)
    {
        var genre = await repository.GetByIdAsync(id);

        return genre?.ToShortDto();
    }

    public async Task<ICollection<GenreShortDto>> GetSubGenresAsync(Guid parentId)
    {
        var subGenres = (await repository.GetAllByFilterAsync(g => g.ParentGenreId == parentId))
            .Select(g => g.ToShortDto())
            .ToList();

        return subGenres;
    }

    public async Task UpdateAsync(UpdateGenreDto dto)
    {
        if (dto.ParentGenreId is not null)
        {
            // ensure that the genre for parentGenre exists
            _ = await repository.GetByIdAsync((Guid)dto.ParentGenreId)
                ?? throw new GenreNotFoundException((Guid)dto.ParentGenreId);
        }

        var genreToUpdate = await repository.GetByIdAsync(dto.Id)
                            ?? throw new GenreNotFoundException(dto.Id);

        dto.UpdateEntity(genreToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGenreDto dto)
    {
        if (dto.ParentGenreId is not null)
        {
            // ensure that the genre for parentGenre exists
            if (!await repository.Exists(g => g.ParentGenreId == dto.ParentGenreId))
            {
                throw new GenreNotFoundException((Guid)dto.ParentGenreId);
            }
        }

        var genreToAdd = dto.ToEntity();

        await repository.CreateAsync(genreToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<ICollection<GenreShortDto>> GetAllByGameKeyAsync(string gameKey)
    {
        var game = (await gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                     throw new GameNotFoundException(gameKey);

        // get genreIds of the game from GamGenre table
        var genreIds = (await gameGenreRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(g => g.GenreId);

        // get genres from ids
        var genres = (await repository.GetAllByFilterAsync(g => genreIds.Contains(g.Id)))
            .Select(g => g.ToShortDto())
            .ToList();

        return genres;
    }
}