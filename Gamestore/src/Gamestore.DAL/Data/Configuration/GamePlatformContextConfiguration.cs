using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class GamePlatformContextConfiguration : IEntityTypeConfiguration<GamePlatform>
{
    public void Configure(EntityTypeBuilder<GamePlatform> builder)
    {
        builder
            .HasKey(gp => new { gp.GameId, gp.PlatformId });
        builder
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(gp => gp.GameId);
        builder
            .HasOne<Platform>()
            .WithMany()
            .HasForeignKey(gp => gp.PlatformId);
    }
}