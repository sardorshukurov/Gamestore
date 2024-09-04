using FluentValidation;
using Gamestore.DAL.Repository;
using PlatformEntity = Gamestore.Domain.Entities.Games.Platform;

namespace Gamestore.BLL.DTOs.Platform;

public record CreatePlatformRequest(
    string Type);

public class CreatePlatformValidator : AbstractValidator<CreatePlatformRequest>
{
    private readonly IRepository<PlatformEntity> _platformRepository;

    public CreatePlatformValidator(IRepository<PlatformEntity> platformRepository)
    {
        _platformRepository = platformRepository;

        RuleFor(p => p.Type)
            .NotEmpty()
            .WithMessage("Platform type is required")
            .Must((type) => BeUniqueType(type).Result)
            .WithMessage("Platform type must be unique");
    }

    private async Task<bool> BeUniqueType(string type)
    {
        return !await _platformRepository.ExistsAsync(p => p.Type == type);
    }
}