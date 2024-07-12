namespace Gamestore.BLL.DTOs.Genre;

public record UpdateGenreDto(
    Guid Id,
    string Name,
    Guid? ParentGenreId,
    ICollection<Guid>? GameIds);