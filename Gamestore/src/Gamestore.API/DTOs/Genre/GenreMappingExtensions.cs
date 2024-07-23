using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.API.DTOs.Genre;

public static class GenreMappingExtensions
{
    public static GenreResponse ToResponse(this GenreDto dto)
    {
        return new GenreResponse(
            dto.Id,
            dto.Name,
            dto.ParentGenreId);
    }

    public static GenreShortResponse ToShortResponse(this GenreDto dto)
    {
        return new GenreShortResponse(
            dto.Id,
            dto.Name);
    }

    public static GenreShortResponse ToShortResponse(this GenreShortDto dto)
    {
        return new GenreShortResponse(
            dto.Id,
            dto.Name);
    }

    public static CreateGenreDto ToDto(this CreateGenreRequest request)
    {
        return new CreateGenreDto(
            request.Genre.Name,
            request.Genre.ParentGenreId,
            null);
    }

    public static UpdateGenreDto ToDto(this UpdateGenreRequest request)
    {
        return new UpdateGenreDto(
            request.Genre.Id,
            request.Genre.Name,
            request.Genre.ParentGenreId,
            null);
    }
}