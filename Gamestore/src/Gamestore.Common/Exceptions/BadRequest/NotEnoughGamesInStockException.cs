namespace Gamestore.Common.Exceptions.BadRequest;

public class NotEnoughGamesInStockException(string message) : BadRequestException(message);