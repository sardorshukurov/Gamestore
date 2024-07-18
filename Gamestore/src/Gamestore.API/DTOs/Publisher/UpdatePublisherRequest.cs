namespace Gamestore.API.DTOs.Publisher;

public record UpdatePublisherRequest(
    Guid Id,
    string CompanyName,
    string HomePage,
    string Description);