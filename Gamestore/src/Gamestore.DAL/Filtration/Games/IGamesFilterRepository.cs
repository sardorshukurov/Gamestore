using Gamestore.Domain.Entities;

namespace Gamestore.DAL.Filtration.Games;

public interface IGamesFilterRepository
{
    Task<ICollection<Game>> GetAsync(SearchCriteria criteria);
}