using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class GameGenreContextConfiguration : IEntityTypeConfiguration<GameGenre>
{
    public void Configure(EntityTypeBuilder<GameGenre> builder)
    {
        builder
            .HasKey(gg => new { gg.GameId, gg.GenreId });
        builder
            .HasOne(gg => gg.Genre)
            .WithMany()
            .HasForeignKey(gg => gg.GenreId);
        builder
            .HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameId);
    }
}