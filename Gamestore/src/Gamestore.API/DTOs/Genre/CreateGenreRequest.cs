using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.API.DTOs.Genre;

public record CreateGenreRequest(
    string Name,
    Guid? ParentGenreId);

public class CreateGenreValidator : AbstractValidator<CreateGenreRequest>
{
    private readonly MainDbContext _dbContext;

    public CreateGenreValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("Genre name is required")
            .Must(BeUniqueGenreName)
            .WithMessage("Genre name must be unique");

        RuleFor(g => g.ParentGenreId)
            .Must(ContainExistingParentGenre)
            .WithMessage("Parent genre must exist");
    }

    private bool BeUniqueGenreName(string name)
    {
        return !_dbContext.Genres.Any(g => g.Name == name);
    }

    private bool ContainExistingParentGenre(Guid? parentGenreId)
    {
        return parentGenreId is null || _dbContext.Genres.Any(g => g.Id == parentGenreId);
    }
}