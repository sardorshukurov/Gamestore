using PublisherEntity = Gamestore.Domain.Entities.Publisher;

namespace Gamestore.BLL.DTOs.Publisher;

public static class PublisherMappingExtensions
{
    public static PublisherResponse ToResponse(this PublisherEntity entity)
    {
        return new PublisherResponse(
            entity.Id,
            entity.CompanyName,
            entity.Description ?? string.Empty,
            entity.HomePage ?? string.Empty);
    }

    public static PublisherShortResponse ToShortResponse(this PublisherEntity entity)
    {
        return new PublisherShortResponse(
            entity.Id,
            entity.CompanyName);
    }

    public static PublisherEntity ToEntity(this CreatePublisherRequest request)
    {
        return new PublisherEntity
        {
            CompanyName = request.CompanyName,
            HomePage = request.HomePage,
            Description = request.Description,
        };
    }

    public static void UpdateEntity(this UpdatePublisherRequest request, PublisherEntity entity)
    {
        entity.Id = request.Id;
        entity.CompanyName = request.CompanyName;
        entity.HomePage = request.HomePage;
        entity.Description = request.Description;
    }
}