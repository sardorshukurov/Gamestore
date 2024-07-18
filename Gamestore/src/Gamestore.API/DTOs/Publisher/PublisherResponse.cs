namespace Gamestore.API.DTOs.Publisher;

public record PublisherResponse(
    Guid Id,
    string CompanyName,
    string Description,
    string HomePage);