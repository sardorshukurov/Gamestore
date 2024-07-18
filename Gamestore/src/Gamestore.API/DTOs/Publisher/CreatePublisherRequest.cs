namespace Gamestore.API.DTOs.Publisher;

public record CreatePublisherRequest(
    string CompanyName,
    string HomePage,
    string Description);