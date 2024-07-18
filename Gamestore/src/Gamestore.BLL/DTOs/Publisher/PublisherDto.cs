namespace Gamestore.BLL.DTOs.Publisher;

public record PublisherDto(
    Guid Id,
    string CompanyName,
    string? Description,
    string? HomePage);