using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

public interface IFilter
{
    Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria);
}