using GenreEntity = Gamestore.Domain.Entities.Genre;

namespace Gamestore.BLL.DTOs.Genre;

public static class GenreMappingExtensions
{
    public static GenreResponse ToResponse(this GenreEntity entity)
    {
        return new GenreResponse(
            entity.Id,
            entity.Name,
            entity.ParentGenreId);
    }

    public static GenreShortResponse ToShortResponse(this GenreEntity entity)
    {
        return new GenreShortResponse(
            entity.Id,
            entity.Name);
    }

    public static GenreEntity ToEntity(this CreateGenreRequest request)
    {
        return new GenreEntity()
        {
            Name = request.Name,
            ParentGenreId = request.ParentGenreId,
        };
    }

    public static void UpdateEntity(this UpdateGenreRequest request, GenreEntity entity)
    {
        entity.Id = request.Id;
        entity.Name = request.Name;
        entity.ParentGenreId = request.ParentGenreId;
    }
}