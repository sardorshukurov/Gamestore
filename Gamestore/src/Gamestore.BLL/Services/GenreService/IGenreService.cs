using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.BLL.Services.GenreService;

public interface IGenreService
{
    Task<ICollection<GenreDto>> GetAllAsync();

    Task<GenreDto> GetByIdAsync(Guid id);

    Task<ICollection<GenreDto>> GetSubGenresAsync(Guid parentId);

    Task UpdateAsync(UpdateGenreDto dto);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreateGenreDto dto);

    Task<ICollection<GenreDto>> GetAllByGameKeyAsync(string gameKey);
}