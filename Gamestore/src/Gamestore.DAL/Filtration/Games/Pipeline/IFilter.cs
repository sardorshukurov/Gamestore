using Gamestore.Domain.Entities.Games;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public interface IFilter
{
    Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria);
}