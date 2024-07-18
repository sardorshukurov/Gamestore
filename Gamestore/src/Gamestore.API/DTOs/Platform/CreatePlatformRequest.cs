using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.API.DTOs.Platform;

public record CreatePlatformRequest(
    string Type);

public class CreatePlatformValidator : AbstractValidator<CreatePlatformRequest>
{
    private readonly MainDbContext _dbContext;

    public CreatePlatformValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(p => p.Type)
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