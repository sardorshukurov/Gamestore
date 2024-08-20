namespace Gamestore.Common.Exceptions.NotFound;

public class PlatformNotFoundException(Guid id) : NotFoundException($"Platform with id: {id} not found");