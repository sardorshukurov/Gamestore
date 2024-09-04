using FluentValidation;
using Gamestore.DAL.Repository;
using PublisherEntity = Gamestore.Domain.Entities.Games.Publisher;

namespace Gamestore.BLL.DTOs.Publisher;

public record CreatePublisherRequest(
    string CompanyName,
    string HomePage,
    string Description);

public class CreatePublisherValidator : AbstractValidator<CreatePublisherRequest>
{
    private readonly IRepository<PublisherEntity> _publisherRepository;

    public CreatePublisherValidator(IRepository<PublisherEntity> publisherRepository)
    {
        _publisherRepository = publisherRepository;

        RuleFor(p => p.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required")
            .Must((companyName) => BeUniqueCompanyName(companyName).Result)
            .WithMessage("Company name must be unique");

        RuleFor(p => p.HomePage)
            .NotEmpty()
            .WithMessage("Home page is required");

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }

    private async Task<bool> BeUniqueCompanyName(string companyName)
    {
        return !await _publisherRepository.ExistsAsync(g => g.CompanyName == companyName);
    }
}