using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class GenreContextConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder
            .HasKey(g => g.Id);
        builder
            .Property(g => g.Name)
            .IsRequired();
        builder
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}