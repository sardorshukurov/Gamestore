using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class CommentContextConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasKey(c => c.Id);
        builder
            .Property(c => c.Name)
            .IsRequired();
        builder
            .Property(c => c.Body)
            .IsRequired();
        builder
            .Property(c => c.ParentCommentId)
            .IsRequired(false);
        builder
            .Property(c => c.GameId)
            .IsRequired();
        builder.HasMany(c => c.Replies)
            .WithOne()
            .HasForeignKey(c => c.ParentCommentId)
            .IsRequired(false);
        builder
            .HasOne<Game>()
            .WithMany(g => g.Comments)
            .HasForeignKey(c => c.GameId);
    }
}