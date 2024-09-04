using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Comments;

public class Comment : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string Body { get; set; }

    public Guid? ParentCommentId { get; set; }

    public Guid GameId { get; set; }

    public virtual ICollection<Comment> Replies { get; set; }
}