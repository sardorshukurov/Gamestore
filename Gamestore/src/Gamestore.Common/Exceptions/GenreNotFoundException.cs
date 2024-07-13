namespace Gamestore.Common.Exceptions;

public class GenreNotFoundException(Guid id) : NotFoundException($"Genre with id: {id} not found");