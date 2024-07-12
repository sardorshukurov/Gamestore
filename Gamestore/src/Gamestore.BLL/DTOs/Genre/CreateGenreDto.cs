namespace Gamestore.BLL.DTOs.Genre;

public record CreateGenreDto(
    string Name,
    Guid? ParentGenreId,
    ICollection<Guid>? GameIds);