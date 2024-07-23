using FluentValidation;
using Gamestore.DAL.Data;

namespace Gamestore.BLL.DTOs.Publisher;

public record UpdatePublisherRequest(
    UpdatePublisher Publisher);

public record UpdatePublisher(
    Guid Id,
    string CompanyName,
    string HomePage,
    string Description);

public class UpdatePublisherValidator : AbstractValidator<UpdatePublisherRequest>
{
    private readonly MainDbContext _dbContext;

    public UpdatePublisherValidator(MainDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(p => p.Publisher.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required")
            .Must(BeUniqueCompanyName)
            .WithMessage("Company name must be unique");

        RuleFor(p => p.Publisher.HomePage)
            .NotEmpty()
            .WithMessage("Home page is required");

        RuleFor(p => p.Publisher.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }

    private bool BeUniqueCompanyName(string companyName)
    {
        return !_dbContext.Publishers.Any(g => g.CompanyName == companyName);
    }
}