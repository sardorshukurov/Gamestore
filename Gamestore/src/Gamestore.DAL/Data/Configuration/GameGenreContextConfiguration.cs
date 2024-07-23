using Gamestore.DAL.Entities;
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
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(gg => gg.GameId);
        builder
            .HasOne<Genre>()
            .WithMany()
            .HasForeignKey(gg => gg.GenreId);
    }
}