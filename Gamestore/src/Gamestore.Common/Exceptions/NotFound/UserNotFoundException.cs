namespace Gamestore.Common.Exceptions.NotFound;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid id)
        : base($"User with id: {id} not found")
    {
    }

    public UserNotFoundException(string email)
        : base($"User with email: {email} not found")
    {
    }
}