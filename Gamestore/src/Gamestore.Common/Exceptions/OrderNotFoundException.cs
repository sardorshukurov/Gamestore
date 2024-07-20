namespace Gamestore.Common.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid id)
        : base($"Open order with id: {id} not found")
    {
    }

    public OrderNotFoundException(Guid customerId, string gameKey)
        : base($"Open order for customer with id: {customerId} and game {gameKey} not found")
    {
    }

    public OrderNotFoundException(string gameKey)
        : base($"Open order with game {gameKey} not found")
    {
    }
}