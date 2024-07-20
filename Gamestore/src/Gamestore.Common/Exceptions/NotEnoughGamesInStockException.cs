namespace Gamestore.Common.Exceptions;

public class NotEnoughGamesInStockException(string message) : Exception(message);