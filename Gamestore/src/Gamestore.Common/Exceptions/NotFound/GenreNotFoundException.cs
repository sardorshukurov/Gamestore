namespace Gamestore.Common.Exceptions.NotFound;

public class GenreNotFoundException(Guid id) : NotFoundException($"Genre with id: {id} not found");