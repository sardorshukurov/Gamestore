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
        builder
            .HasOne<Comment>()
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId);
        builder
            .HasOne<Game>()
            .WithMany(g => g.Comments)
            .HasForeignKey(c => c.GameId);
    }
}