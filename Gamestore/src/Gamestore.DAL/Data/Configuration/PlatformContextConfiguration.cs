using Gamestore.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class PlatformContextConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder
            .HasKey(p => p.Id);
        builder
            .Property(p => p.Type)
            .IsRequired();
        builder
            .HasIndex(p => p.Type)
            .IsUnique();
    }
}