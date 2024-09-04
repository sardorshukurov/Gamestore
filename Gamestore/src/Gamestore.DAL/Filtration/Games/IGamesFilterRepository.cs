using Gamestore.Domain.Entities.Games;

namespace Gamestore.DAL.Filtration.Games;

public interface IGamesFilterRepository
{
    Task<ICollection<Game>> GetAsync(SearchCriteria criteria);
}