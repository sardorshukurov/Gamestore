using Gamestore.BLL.DTOs.Game;

namespace Gamestore.BLL.Services.GameService;

public interface IGameService
{
    Task<ICollection<GameResponse>> GetAllAsync();

    Task<GameResponse?> GetByKeyAsync(string key);

    Task<ICollection<GameResponse>> GetByGenreAsync(Guid genreId);

    Task<ICollection<GameResponse>> GetByPlatformAsync(Guid platformId);

    Task<ICollection<GameResponse>> GetByPublisherAsync(string companyName);

    Task<GameResponse?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdateGameRequest request);

    Task CreateAsync(CreateGameRequest request);

    Task DeleteByKeyAsync(string key);
}