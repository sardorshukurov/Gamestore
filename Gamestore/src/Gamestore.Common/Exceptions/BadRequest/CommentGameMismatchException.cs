namespace Gamestore.Common.Exceptions.BadRequest;

public class CommentGameMismatchException() : BadRequestException($"Parent comment was commented on different game");