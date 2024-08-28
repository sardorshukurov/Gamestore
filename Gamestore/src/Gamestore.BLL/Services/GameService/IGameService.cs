using Gamestore.BLL.DTOs.Game;
using Gamestore.DAL.Filtration.Games;

namespace Gamestore.BLL.Services.GameService;

public interface IGameService
{
    Task<ICollection<GameResponse>> GetFromBothDBsAsync();

    Task<GamesResponse> GetAllAsync(SearchCriteria criteria);

    Task<GameResponse?> GetByKeyAsync(string key);

    Task<ICollection<GameResponse>> GetByGenreAsync(Guid genreId);

    Task<ICollection<GameResponse>> GetByPlatformAsync(Guid platformId);

    Task<ICollection<GameResponse>> GetByPublisherAsync(string companyName);

    Task<GameResponse?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdateGameRequest request);

    Task CreateAsync(CreateGameRequest request);

    Task DeleteByKeyAsync(string key);
}