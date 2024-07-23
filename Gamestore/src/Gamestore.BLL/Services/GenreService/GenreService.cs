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
    public async Task<ICollection<GenreShortResponse>> GetAllAsync()
    {
        var genres = (await repository.GetAllAsync())
            .Select(g => g.ToShortResponse())
            .ToList();

        return genres;
    }

    public async Task<GenreShortResponse?> GetByIdAsync(Guid id)
    {
        var genre = await repository.GetByIdAsync(id);

        return genre?.ToShortResponse();
    }

    public async Task<ICollection<GenreShortResponse>> GetSubGenresAsync(Guid parentId)
    {
        var subGenres = (await repository.GetAllByFilterAsync(g => g.ParentGenreId == parentId))
            .Select(g => g.ToShortResponse())
            .ToList();

        return subGenres;
    }

    public async Task UpdateAsync(UpdateGenreRequest request)
    {
        if (request.Genre.ParentGenreId is not null)
        {
            // ensure that the genre for parentGenre exists
            _ = await repository.GetByIdAsync((Guid)request.Genre.ParentGenreId)
                ?? throw new GenreNotFoundException((Guid)request.Genre.ParentGenreId);
        }

        var genreToUpdate = await repository.GetByIdAsync(request.Genre.Id)
                            ?? throw new GenreNotFoundException(request.Genre.Id);

        request.UpdateEntity(genreToUpdate);

        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteByIdAsync(id);
        await repository.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateGenreRequest request)
    {
        if (request.Genre.ParentGenreId is not null)
        {
            // ensure that the genre for parentGenre exists
            if (!await repository.Exists(g => g.ParentGenreId == request.Genre.ParentGenreId))
            {
                throw new GenreNotFoundException((Guid)request.Genre.ParentGenreId);
            }
        }

        var genreToAdd = request.ToEntity();

        await repository.CreateAsync(genreToAdd);
        await repository.SaveChangesAsync();
    }

    public async Task<ICollection<GenreShortResponse>> GetAllByGameKeyAsync(string gameKey)
    {
        var game = (await gameRepository.GetOneAsync(g => g.Key == gameKey)) ??
                     throw new GameNotFoundException(gameKey);

        // get genreIds of the game from GamGenre table
        var genreIds = (await gameGenreRepository.GetAllByFilterAsync(gg => gg.GameId == game.Id))
            .Select(g => g.GenreId);

        // get genres from ids
        var genres = (await repository.GetAllByFilterAsync(g => genreIds.Contains(g.Id)))
            .Select(g => g.ToShortResponse())
            .ToList();

        return genres;
    }
}