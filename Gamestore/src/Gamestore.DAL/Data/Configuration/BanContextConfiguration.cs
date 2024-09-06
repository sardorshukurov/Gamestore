using Gamestore.Domain.Entities.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class BanContextConfiguration : IEntityTypeConfiguration<Ban>
{
    public void Configure(EntityTypeBuilder<Ban> builder)
    {
        builder
            .HasKey(b => b.Id);
        builder
            .Property(b => b.UserId)
            .IsRequired();
        builder
            .Property(b => b.Duration)
            .IsRequired();
    }
}