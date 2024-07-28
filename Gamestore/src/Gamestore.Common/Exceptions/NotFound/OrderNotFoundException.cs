namespace Gamestore.Common.Exceptions;

public class OrderNotFoundException(string message) : NotFoundException(message);