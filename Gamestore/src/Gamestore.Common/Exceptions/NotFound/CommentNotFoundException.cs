namespace Gamestore.Common.Exceptions.NotFound;

public class CommentNotFoundException(Guid id) : NotFoundException($"Comment with id: {id} not found");
