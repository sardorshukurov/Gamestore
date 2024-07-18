namespace Gamestore.BLL.DTOs.Publisher;

public record UpdatePublisherDto(
    Guid Id,
    string CompanyName,
    string HomePage,
    string Description);