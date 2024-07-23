using PublisherEntity = Gamestore.DAL.Entities.Publisher;

namespace Gamestore.BLL.DTOs.Publisher;

public static class PublisherMappingExtensions
{
    public static PublisherDto ToDto(this PublisherEntity publisher)
    {
        return new PublisherDto(
            publisher.Id,
            publisher.CompanyName,
            publisher.Description,
            publisher.HomePage);
    }

    public static PublisherShortDto ToShortDto(this PublisherEntity publisher)
    {
        return new PublisherShortDto(
            publisher.Id,
            publisher.CompanyName);
    }

    public static PublisherEntity ToEntity(this CreatePublisherDto dto)
    {
        return new PublisherEntity
        {
            CompanyName = dto.CompanyName,
            Description = dto.Description,
            HomePage = dto.HomePage,
        };
    }

    public static void UpdateEntity(this UpdatePublisherDto dto, PublisherEntity publisher)
    {
        publisher.CompanyName = dto.CompanyName;
        publisher.Description = dto.Description;
        publisher.HomePage = dto.HomePage;
    }
}