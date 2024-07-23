using Gamestore.BLL.DTOs.Publisher;

namespace Gamestore.API.DTOs.Publisher;

public static class PublisherMappingExtensions
{
    public static PublisherResponse ToResponse(this PublisherDto dto)
    {
        return new PublisherResponse(
            dto.Id,
            dto.CompanyName,
            dto.Description ?? string.Empty,
            dto.HomePage ?? string.Empty);
    }

    public static PublisherShortResponse ToShortResponse(this PublisherDto dto)
    {
        return new PublisherShortResponse(
            dto.Id,
            dto.CompanyName);
    }

    public static PublisherShortResponse ToShortResponse(this PublisherShortDto dto)
    {
        return new PublisherShortResponse(
            dto.Id,
            dto.CompanyName);
    }

    public static CreatePublisherDto ToDto(this CreatePublisherRequest request)
    {
        return new CreatePublisherDto(
            request.Publisher.CompanyName,
            request.Publisher.HomePage,
            request.Publisher.Description);
    }

    public static UpdatePublisherDto ToDto(this UpdatePublisherRequest request)
    {
        return new UpdatePublisherDto(
            request.Publisher.Id,
            request.Publisher.CompanyName,
            request.Publisher.HomePage,
            request.Publisher.Description);
    }
}