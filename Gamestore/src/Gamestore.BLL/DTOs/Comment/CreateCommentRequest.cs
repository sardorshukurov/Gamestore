using FluentValidation;
using Gamestore.DAL.Repository;
using CommentEntity = Gamestore.Domain.Entities.Comments.Comment;

namespace Gamestore.BLL.DTOs.Comment;

public record CreateCommentRequest(
    string Body,
    Guid? ParentId,
    CommentAction? Action);

public class CreateCommentValidator : AbstractValidator<CreateCommentRequest>
{
    private readonly IRepository<CommentEntity> _commentRepository;

    public CreateCommentValidator(
        IRepository<CommentEntity> commentRepository)
    {
        _commentRepository = commentRepository;

        RuleFor(c => c.Body)
            .NotEmpty()
            .WithMessage("Comment body is required");

        RuleFor(c => c.ParentId)
            .Must((parentId) => Exist(parentId).Result)
            .WithMessage("Parent comment does not exist");
    }

    private async Task<bool> Exist(Guid? commentId)
    {
        return commentId is null || await _commentRepository.ExistsAsync(g => g.Id == commentId);
    }
}