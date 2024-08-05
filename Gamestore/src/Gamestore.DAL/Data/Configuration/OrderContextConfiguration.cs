using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class OrderContextConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasKey(o => o.Id);
        builder
            .Property(o => o.CustomerId)
            .IsRequired();
        builder
            .Property(o => o.Status)
            .IsRequired();
    }
}