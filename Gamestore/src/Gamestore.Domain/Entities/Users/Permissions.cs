namespace Gamestore.Domain.Entities.Users;

[Flags]
public enum Permissions
{
    None = 0,
    AddGame = 1,
    DeleteGame = 2,
    ViewGame = 4,
    UpdateGame = 8,
}
