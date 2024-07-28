namespace Gamestore.Common.Exceptions;

public class CommentGameMismatchException() : BadRequestException($"Parent comment was commented on different game");