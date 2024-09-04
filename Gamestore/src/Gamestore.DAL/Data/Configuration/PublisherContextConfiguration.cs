using Gamestore.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class PublisherContextConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder
            .HasKey(p => p.Id);
        builder
            .Property(p => p.CompanyName)
            .IsRequired();
        builder
            .HasIndex(p => p.CompanyName)
            .IsUnique();
        builder
            .Property(p => p.HomePage)
            .IsRequired(false);
        builder
            .Property(p => p.Description)
            .IsRequired(false);
    }
}