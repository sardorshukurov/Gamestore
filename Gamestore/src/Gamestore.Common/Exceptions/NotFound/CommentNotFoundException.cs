namespace Gamestore.Common.Exceptions;

public class CommentNotFoundException(Guid id) : NotFoundException($"Comment with id: {id} not found");
