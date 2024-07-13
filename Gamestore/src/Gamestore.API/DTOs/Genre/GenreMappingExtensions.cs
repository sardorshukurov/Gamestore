using Gamestore.API.DTOs.Game;
using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.API.DTOs.Genre;

public static class GenreMappingExtensions
{
    public static GenreResponse AsResponse(this GenreDto dto, ICollection<GameShortResponse> games)
    {
        return new GenreResponse(
            dto.Id,
            dto.Name,
            dto.ParentGenreName,
            games);
    }

    public static GenreShortResponse AsShortResponse(this GenreDto dto)
    {
        return new GenreShortResponse(
            dto.Id,
            dto.Name);
    }

    public static CreateGenreDto AsEntity(this CreateGenreRequest request)
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