using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.BLL.Services.GenreService;

public interface IGenreService
{
    Task<ICollection<GenreShortDto>> GetAllAsync();

    Task<GenreShortDto?> GetByIdAsync(Guid id);

    Task<ICollection<GenreShortDto>> GetSubGenresAsync(Guid parentId);

    Task UpdateAsync(UpdateGenreDto dto);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreateGenreDto dto);

    Task<ICollection<GenreShortDto>> GetAllByGameKeyAsync(string gameKey);
}