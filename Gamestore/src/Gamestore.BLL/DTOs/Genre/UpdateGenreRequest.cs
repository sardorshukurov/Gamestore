using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.BLL.DTOs.Genre;

public record UpdateGenreRequest(
    UpdateGenre Genre);

public record UpdateGenre(
    Guid Id,
    string Name,
    Guid? ParentGenreId);

public class UpdateGenreValidator : AbstractValidator<UpdateGenreRequest>
{
    private readonly MainDbContext _dbContext;

    public UpdateGenreValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(g => g.Genre.Name)
            .NotEmpty()
            .WithMessage("Genre name is required")
            .Must(BeUniqueGenreName)
            .WithMessage("Genre name must be unique");

        RuleFor(g => g.Genre.ParentGenreId)
            .Must(ContainExistingParentGenre)
            .WithMessage("Parent genre must exist");
    }

    private bool BeUniqueGenreName(string name)
    {
        return !_dbContext.Genres.Any(g => g.Name == name);
    }

    private bool ContainExistingParentGenre(Guid? parentGenreId)
    {
        if (parentGenreId == null)
        {
            return true;
        }

        var parentGenre = _dbContext.Genres.FirstOrDefault(g => g.Id == parentGenreId);

        return parentGenre is not null || parentGenre.Id != parentGenre.ParentGenreId;
    }
}