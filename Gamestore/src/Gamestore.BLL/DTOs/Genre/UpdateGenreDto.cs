namespace Gamestore.BLL.DTOs.Genre;

public record UpdateGenreDto(
    string Name,
    Guid? ParentGenreId,
    ICollection<Guid>? GameIds);