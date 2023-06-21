using Microsoft.EntityFrameworkCore;

namespace Persistance.Configurations;

public class LikeConfigure : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder
         .HasOne(l => l.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.PostId)
         .OnDelete(DeleteBehavior.NoAction);
        builder
        .HasOne(l => l.Comment)
        .WithMany(p => p.Likes)
        .HasForeignKey(l => l.CommentId)
        .OnDelete(DeleteBehavior.NoAction);
        builder.Property(l => l.IsActived).HasDefaultValue(true);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);
         
    }
}
