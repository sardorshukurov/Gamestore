using FluentValidation;
using Gamestore.DAL.Repository;
using GenreEntity = Gamestore.Domain.Entities.Genre;

namespace Gamestore.BLL.DTOs.Genre;

public record UpdateGenreRequest(
    Guid Id,
    string Name,
    Guid? ParentGenreId);

public class UpdateGenreValidator : AbstractValidator<UpdateGenreRequest>
{
    private readonly IRepository<GenreEntity> _genreRepository;

    public UpdateGenreValidator(IRepository<GenreEntity> genreRepository)
    {
        _genreRepository = genreRepository;

        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("Genre name is required")
            .Must((name) => BeUniqueGenreName(name).Result)
            .WithMessage("Genre name must be unique");

        RuleFor(g => g.ParentGenreId)
            .Must((parentGenreId) => ContainExistingParentGenre(parentGenreId).Result)
            .WithMessage("Parent genre must exist");
    }

    private async Task<bool> BeUniqueGenreName(string name)
    {
        return !await _genreRepository.ExistsAsync(g => g.Name == name);
    }

    private async Task<bool> ContainExistingParentGenre(Guid? parentGenreId)
    {
        if (parentGenreId == null)
        {
            return true;
        }

        var parentGenre = await _genreRepository.GetOneAsync(g => g.Id == parentGenreId);

        return parentGenre is not null || parentGenre.Id != parentGenre.ParentGenreId;
    }
}