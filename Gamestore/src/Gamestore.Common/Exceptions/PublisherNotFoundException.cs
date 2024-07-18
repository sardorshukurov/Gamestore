namespace Gamestore.Common.Exceptions;

public class PublisherNotFoundException(Guid id) : NotFoundException($"Publisher with id: {id} not found");
