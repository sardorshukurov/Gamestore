using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.BLL.DTOs.Platform;

public record UpdatePlatformRequest(
    UpdatePlatform Platform);

public record UpdatePlatform(
    Guid Id,
    string Type);

public class UpdatePlatformValidator : AbstractValidator<UpdatePlatformRequest>
{
    private readonly MainDbContext _dbContext;

    public UpdatePlatformValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(p => p.Platform.Type)
            .NotEmpty()
            .WithMessage("Platform type is required")
            .Must(BeUniqueType)
            .WithMessage("Platform type must be unique");
    }

    private bool BeUniqueType(string type)
    {
        return !_dbContext.Platforms.Any(p => p.Type == type);
    }
}