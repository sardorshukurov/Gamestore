using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class GameContextConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasKey(g => g.Id);
        builder
            .Property(g => g.Name)
            .IsRequired();
        builder
            .HasIndex(g => g.Key)
            .IsUnique();
        builder
            .Property(g => g.Description)
            .IsRequired(false);
        builder
            .Property(g => g.Price)
            .IsRequired();
        builder
            .Property(g => g.UnitInStock)
            .IsRequired();
        builder
            .Property(g => g.Discount)
            .IsRequired();
        builder
            .Property(g => g.PublisherId)
            .IsRequired();
        builder
            .HasOne<Publisher>()
            .WithMany()
            .HasForeignKey(g => g.PublisherId);
    }
}