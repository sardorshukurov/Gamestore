namespace Gamestore.Common.Exceptions.NotFound;

public class UserRoleNotFoundException(Guid id) : NotFoundException($"User role with id: {id} not found");
