using Gamestore.BLL.DTOs.Game;
using GenreEntity = Gamestore.Domain.Entities.Genre;

namespace Gamestore.BLL.DTOs.Genre;

public static class GenreMappingExtensions
{
    public static GenreDto ToDto(this GenreEntity genre, ICollection<GameShortDto> games, GenreShortDto? parentGenre)
    {
        return new GenreDto(
            genre.Id,
            genre.Name,
            parentGenre?.Id,
            parentGenre?.Name,
            games);
    }

    public static GenreShortDto ToShortDto(this GenreEntity genre)
    {
        return new GenreShortDto(
            genre.Id,
            genre.Name,
            genre.ParentGenreId);
    }

    public static GenreEntity ToEntity(this CreateGenreDto dto)
    {
        return new GenreEntity
        {
            Name = dto.Name,
            ParentGenreId = dto.ParentGenreId,
        };
    }

    public static void UpdateEntity(this UpdateGenreDto dto, GenreEntity genre)
    {
        genre.Name = dto.Name;
        genre.ParentGenreId = dto.ParentGenreId;
    }
}