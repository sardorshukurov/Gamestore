namespace Gamestore.Common.Exceptions;

public class PlatformNotFoundException(Guid id) : NotFoundException($"Platform with id: {id} not found");