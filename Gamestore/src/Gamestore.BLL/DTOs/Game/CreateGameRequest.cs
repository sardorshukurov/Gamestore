using FluentValidation;
using Gamestore.DAL.Repository;
using GameEntity = Gamestore.Domain.Entities.Game;
using GenreEntity = Gamestore.Domain.Entities.Genre;
using PlatformEntity = Gamestore.Domain.Entities.Platform;
using PublisherEntity = Gamestore.Domain.Entities.Publisher;

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
    private readonly IRepository<GenreEntity> _genreRepository;
    private readonly IRepository<GameEntity> _gameRepository;
    private readonly IRepository<PlatformEntity> _platformRepository;
    private readonly IRepository<PublisherEntity> _publisherRepository;

    public CreateGameValidator(
        IRepository<GenreEntity> genreRepository,
        IRepository<GameEntity> gameRepository,
        IRepository<PlatformEntity> platformRepository,
        IRepository<PublisherEntity> publisherRepository)
    {
        _genreRepository = genreRepository;
        _gameRepository = gameRepository;
        _platformRepository = platformRepository;
        _publisherRepository = publisherRepository;

        RuleFor(g => g.Game.Name)
            .NotEmpty()
            .WithMessage("Game name is required");

        RuleFor(g => g.Game.Key)
            .NotEmpty()
            .WithMessage("Key is required")
            .Must((gameKey) => BeUniqueKey(gameKey).Result)
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
            .Must((genreIds) => ContainExistingGenres(genreIds).Result)
            .WithMessage("All genres must be existing genres");

        RuleFor(p => p.Platforms)
            .Must((platformsIds) => ContainExistingPlatforms(platformsIds).Result)
            .WithMessage("All platforms must be existing platforms");

        RuleFor(p => p.Publisher)
            .Must((publishersIds) => ContainExistingPublisher(publishersIds).Result)
            .WithMessage("Publisher must be existing publisher");
    }

    private async Task<bool> ContainExistingGenres(ICollection<Guid> genreIds)
    {
        if (genreIds.Contains(Guid.Empty))
        {
            return false;
        }

        var existingGenreIds = await _genreRepository
            .GetAllByFilterAsync(g => genreIds.Contains(g.Id));

        return existingGenreIds.Count() == genreIds.Count;
    }

    private async Task<bool> ContainExistingPlatforms(ICollection<Guid> platformIds)
    {
        if (platformIds.Contains(Guid.Empty))
        {
            return false;
        }

        var existingPlatformIds = await _platformRepository
            .GetAllByFilterAsync(p => platformIds.Contains(p.Id));

        return existingPlatformIds.Count() == platformIds.Count;
    }

    private async Task<bool> ContainExistingPublisher(Guid publisherId)
    {
        return publisherId != Guid.Empty && await _publisherRepository.ExistsAsync(p => p.Id == publisherId);
    }

    private async Task<bool> BeUniqueKey(string key)
    {
        return !await _gameRepository.ExistsAsync(g => g.Key == key);
    }
}