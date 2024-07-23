using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.BLL.DTOs.Game;

public record CreateGameRequest(
    CreateGame Game,
    ICollection<Guid> Genres,
    ICollection<Guid> Platforms,
    Guid Publisher);

public record CreateGame(
    string Name,
    string Key,
    string Description,
    double Price,
    int UnitInStock,
    int Discount);

public class CreateGameValidator : AbstractValidator<CreateGameRequest>
{
    private readonly MainDbContext _dbContext;

    public CreateGameValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(g => g.Game.Name)
            .NotEmpty()
            .WithMessage("Game name is required");

        RuleFor(g => g.Game.Key)
            .NotEmpty()
            .WithMessage("Key is required")
            .Must(BeUniqueKey)
            .WithMessage("Key must be unique");

        RuleFor(g => g.Game.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(g => g.Game.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(g => g.Game.UnitInStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit in Stock must be greater than or equal to 0");

        RuleFor(g => g.Game.Discount)
            .InclusiveBetween(0, 100)
            .WithMessage("Discount must be between 0 and 100");

        RuleFor(g => g.Genres)
            .Must(ContainExistingGenres)
            .WithMessage("All genres must be existing genres");

        RuleFor(p => p.Platforms)
            .Must(ContainExistingPlatforms)
            .WithMessage("All platforms must be existing platforms");

        RuleFor(p => p.Publisher)
            .Must(ContainExistingPublisher)
            .WithMessage("Publisher must be existing publisher");
    }

    private bool ContainExistingGenres(ICollection<Guid> genreIds)
    {
        if (genreIds.Contains(Guid.Empty))
        {
            return false;
        }

        var existingGenreIds = _dbContext.Genres
            .Where(g => genreIds.Contains(g.Id))
            .Select(g => g.Id)
            .ToList();

        return existingGenreIds.Count == genreIds.Count;
    }

    private bool ContainExistingPlatforms(ICollection<Guid> platformIds)
    {
        if (platformIds.Contains(Guid.Empty))
        {
            return false;
        }

        var existingPlatformIds = _dbContext.Platforms
            .Where(p => platformIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToList();

        return existingPlatformIds.Count == platformIds.Count;
    }

    private bool ContainExistingPublisher(Guid publisherId)
    {
        return publisherId != Guid.Empty && _dbContext.Publishers.Any(p => p.Id == publisherId);
    }

    private bool BeUniqueKey(string key)
    {
        return !_dbContext.Games.Any(g => g.Key == key);
    }
}