using Domain;

namespace Persistance;

public class LikeCommentConfigure : IEntityTypeConfiguration<LikeComment>
{
    public void Configure(EntityTypeBuilder<LikeComment> builder)
    {
        builder
         .HasOne(l => l.Comment)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.CommentId)
         .OnDelete(DeleteBehavior.Restrict);


        builder.Property(l => l.IsActived).HasDefaultValue(true);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

    }


}
