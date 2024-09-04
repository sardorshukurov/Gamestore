using Gamestore.Domain.Entities.Games;
using Gamestore.Domain.Entities.Orders;
using Gamestore.Domain.Entities.Users;

namespace Gamestore.DAL.Data.Seeder;

public static class BaseData
{
    public static readonly IEnumerable<Platform> Platforms =
    [
        new Platform
        {
            Type = "Mobile",
        },
        new Platform
        {
            Type = "Browser",
        },
        new Platform
        {
            Type = "Desktop",
        },
        new Platform
        {
            Type = "Console",
        },
    ];

    public static readonly IEnumerable<Genre> ParentGenres =
    [
        new Genre
        {
            Name = "Strategy",
        },
        new Genre
        {
            Name = "RPG",
        },
        new Genre
        {
            Name = "Sports",
        },
        new Genre
        {
            Name = "Races",
        },
        new Genre
        {
            Name = "Action",
        },
        new Genre
        {
            Name = "Adventure",
        },
        new Genre
        {
            Name = "Puzzle & Skill",
        },
    ];

    public static readonly IEnumerable<Genre> ChildGenres =
    [
        new Genre
        {
            Name = "RTS",
            ParentGenreId = ParentGenres.First(p => p.Name == "Strategy").Id,
        },
        new Genre
        {
            Name = "TBS",
            ParentGenreId = ParentGenres.First(p => p.Name == "Strategy").Id,
        },
        new Genre
        {
            Name = "Rally",
            ParentGenreId = ParentGenres.First(p => p.Name == "Races").Id,
        },
        new Genre
        {
            Name = "Arcade",
            ParentGenreId = ParentGenres.First(p => p.Name == "Races").Id,
        },
        new Genre
        {
            Name = "Formula",
            ParentGenreId = ParentGenres.First(p => p.Name == "Races").Id,
        },
        new Genre
        {
            Name = "Off-road",
            ParentGenreId = ParentGenres.First(p => p.Name == "Races").Id,
        },
        new Genre
        {
            Name = "FPS",
            ParentGenreId = ParentGenres.First(p => p.Name == "Action").Id,
        },
        new Genre
        {
            Name = "TPS",
            ParentGenreId = ParentGenres.First(p => p.Name == "Action").Id,
        },
    ];

    public static readonly IEnumerable<PaymentMethod> PaymentMethods =
    [
        new PaymentMethod
        {
            ImageUrl = "https://cdn-icons-png.flaticon.com/512/858/858170.png",
            Title = "Bank",
            Description = "Bank Payment",
        },
        new PaymentMethod
        {
            ImageUrl = "https://cdn-icons-png.flaticon.com/512/11105/11105672.png",
            Title = "IBox terminal",
            Description = "IBox terminal payment",
        },
        new PaymentMethod
        {
            ImageUrl = "https://static-00.iconduck.com/assets.00/visa-icon-2048x628-6yzgq2vq.png",
            Title = "Visa",
            Description = "Visa Payment",
        },
    ];

    public static readonly IEnumerable<UserRole> UserRoles =
    [
        new UserRole
        {
            Name = "Admin",
            Permissions =
            Permissions.AddGame
            | Permissions.DeleteGame
            | Permissions.UpdateGame
            | Permissions.ViewGame,
        },
        new UserRole
        {
            Name = "Manager",
            Permissions =
            Permissions.AddGame
            | Permissions.UpdateGame
            | Permissions.ViewGame,
        },
    ];
}