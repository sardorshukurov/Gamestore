using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.BLL.Services.GenreService;

public interface IGenreService
{
    Task<ICollection<GenreShortResponse>> GetAllAsync();

    Task<GenreShortResponse?> GetByIdAsync(Guid id);

    Task<ICollection<GenreShortResponse>> GetSubGenresAsync(Guid parentId);

    Task UpdateAsync(UpdateGenreRequest request);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreateGenreRequest request);

    Task<ICollection<GenreShortResponse>> GetAllByGameKeyAsync(string gameKey);
}