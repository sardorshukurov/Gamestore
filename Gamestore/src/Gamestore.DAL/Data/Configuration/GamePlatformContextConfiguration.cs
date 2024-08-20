using Gamestore.Domain.Entities;
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
            .HasOne(gp => gp.Platform)
            .WithMany()
            .HasForeignKey(gp => gp.PlatformId);
        builder
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(gp => gp.GameId);
    }
}