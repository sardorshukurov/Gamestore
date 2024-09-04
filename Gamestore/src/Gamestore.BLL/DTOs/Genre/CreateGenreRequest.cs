using FluentValidation;
using Gamestore.DAL.Repository;
using GenreEntity = Gamestore.Domain.Entities.Games.Genre;

namespace Gamestore.BLL.DTOs.Genre;

public record CreateGenreRequest(
    string Name,
    Guid? ParentGenreId);

public class CreateGenreValidator : AbstractValidator<CreateGenreRequest>
{
    private readonly IRepository<GenreEntity> _genreRepository;

    public CreateGenreValidator(IRepository<GenreEntity> genreRepository)
    {
        _genreRepository = genreRepository;

        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("Genre name is required")
            .Must((genreName) => BeUniqueGenreName(genreName).Result)
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
        return parentGenreId is null || await _genreRepository.ExistsAsync(g => g.Id == parentGenreId);
    }
}