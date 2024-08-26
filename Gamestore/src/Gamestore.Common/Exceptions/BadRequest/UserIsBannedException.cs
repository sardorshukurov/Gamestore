namespace Gamestore.Common.Exceptions.BadRequest;

public class UserIsBannedException(string userName) : BadRequestException($"User {userName} is banned.");