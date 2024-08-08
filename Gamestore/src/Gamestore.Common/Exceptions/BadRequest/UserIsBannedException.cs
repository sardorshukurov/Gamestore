namespace Gamestore.Common.Exceptions;

public class UserIsBannedException(string userName) : BadRequestException($"User {userName} is banned.");