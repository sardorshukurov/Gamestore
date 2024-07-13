using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class GenreContextConfiguration
{
    public static void ConfigureGenres(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>()
            .HasKey(g => g.Id);
        modelBuilder.Entity<Genre>()
            .Property(g => g.Name)
            .IsRequired();
        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}