using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.API.DTOs.Genre;

public static class GenreMappingExtensions
{
    public static GenreResponse AsResponse(this GenreDto dto)
    {
        return new GenreResponse(
            dto.Id,
            dto.Name,
            dto.ParentGenreId);
    }

    public static GenreShortResponse AsShortResponse(this GenreDto dto)
    {
        return new GenreShortResponse(
            dto.Id,
            dto.Name);
    }

    public static GenreShortResponse AsShortResponse(this GenreShortDto dto)
    {
        return new GenreShortResponse(
            dto.Id,
            dto.Name);
    }

    public static CreateGenreDto AsDto(this CreateGenreRequest request)
    {
        return new CreateGenreDto(
            request.Name,
            request.ParentGenreId,
            null);
    }

    public static UpdateGenreDto AsDto(this UpdateGenreRequest request)
    {
        return new UpdateGenreDto(
            request.Id,
            request.Name,
            request.ParentGenreId,
            null);
    }
}