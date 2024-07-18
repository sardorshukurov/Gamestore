using Gamestore.BLL.DTOs.Publisher;

namespace Gamestore.API.DTOs.Publisher;

public static class PublisherMappingExtensions
{
    public static PublisherResponse AsResponse(this PublisherDto dto)
    {
        return new PublisherResponse(
            dto.Id,
            dto.CompanyName,
            dto.Description ?? string.Empty,
            dto.HomePage ?? string.Empty);
    }

    public static PublisherShortResponse AsShortResponse(this PublisherDto dto)
    {
        return new PublisherShortResponse(
            dto.Id,
            dto.CompanyName);
    }

    public static PublisherShortResponse AsShortResponse(this PublisherShortDto dto)
    {
        return new PublisherShortResponse(
            dto.Id,
            dto.CompanyName);
    }

    public static CreatePublisherDto AsDto(this CreatePublisherRequest request)
    {
        return new CreatePublisherDto(
            request.Publisher.CompanyName,
            request.Publisher.HomePage,
            request.Publisher.Description);
    }

    public static UpdatePublisherDto AsDto(this UpdatePublisherRequest request)
    {
        return new UpdatePublisherDto(
            request.Publisher.Id,
            request.Publisher.CompanyName,
            request.Publisher.HomePage,
            request.Publisher.Description);
    }
}