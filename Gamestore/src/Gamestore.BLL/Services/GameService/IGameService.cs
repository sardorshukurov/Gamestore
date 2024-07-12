using Gamestore.BLL.DTOs.Game;

namespace Gamestore.BLL.Services.GameService;

public interface IGameService
{
    Task<ICollection<GameDto>> GetAllAsync();

    Task<GameDto?> GetByKeyAsync(string key);

    Task<ICollection<GameDto>> GetByGenreAsync(Guid genreId);

    Task<ICollection<GameDto>> GetByPlatformAsync(Guid platformId);

    Task<GameDto?> GetByIdAsync(Guid id);

    Task UpdateAsync(Guid id, UpdateGameDto dto);

    Task CreateAsync(CreateGameDto dto);

    Task DeleteByKeyAsync(string key);
}