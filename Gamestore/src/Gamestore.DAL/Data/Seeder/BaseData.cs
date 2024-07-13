using Gamestore.DAL.Entities;

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
}