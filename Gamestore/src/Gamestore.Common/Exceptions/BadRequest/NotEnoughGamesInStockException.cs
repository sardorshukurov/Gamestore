namespace Gamestore.Common.Exceptions;

public class NotEnoughGamesInStockException(string message) : BadRequestException(message);