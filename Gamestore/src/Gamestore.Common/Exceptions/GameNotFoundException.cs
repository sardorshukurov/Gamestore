namespace Gamestore.Common.Exceptions;

public class GameNotFoundException : NotFoundException
{
    public GameNotFoundException(Guid id)
        : base($"Game with id: {id} not found")
    {
    }

    public GameNotFoundException(string gameKey)
        : base($"Game with key: {gameKey} not found")
    {
    }
}