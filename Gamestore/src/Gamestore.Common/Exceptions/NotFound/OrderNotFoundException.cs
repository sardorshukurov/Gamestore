namespace Gamestore.Common.Exceptions.NotFound;

public class OrderNotFoundException(string message) : NotFoundException(message);