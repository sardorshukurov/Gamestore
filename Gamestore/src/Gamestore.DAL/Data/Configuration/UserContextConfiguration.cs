using Gamestore.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class UserContextConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);
        builder
            .HasIndex(u => u.Email)
            .IsUnique();
        builder
            .Property(u => u.FirstName)
            .IsRequired();
        builder
            .Property(u => u.LastName)
            .IsRequired();
        builder
            .HasMany(u => u.Roles)
            .WithMany();
    }
}
