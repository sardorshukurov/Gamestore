namespace Gamestore.BLL.DTOs.Publisher;

public record CreatePublisherDto(
    string CompanyName,
    string HomePage,
    string Description);