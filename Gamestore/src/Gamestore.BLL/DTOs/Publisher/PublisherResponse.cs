namespace Gamestore.BLL.DTOs.Publisher;

public record PublisherResponse(
    Guid Id,
    string CompanyName,
    string Description,
    string HomePage);