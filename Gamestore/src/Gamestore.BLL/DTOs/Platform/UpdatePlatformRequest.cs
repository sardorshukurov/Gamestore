using FluentValidation;
using Gamestore.DAL.Repository;
using PlatformEntity = Gamestore.Domain.Entities.Platform;

namespace Gamestore.BLL.DTOs.Platform;

public record UpdatePlatformRequest(
    Guid Id,
    string Type);

public class UpdatePlatformValidator : AbstractValidator<UpdatePlatformRequest>
{
    private readonly IRepository<PlatformEntity> _platformRepository;

    public UpdatePlatformValidator(IRepository<PlatformEntity> platformRepository)
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
        return !await _platformRepository.Exists(p => p.Type == type);
    }
}