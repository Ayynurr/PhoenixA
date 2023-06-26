using Domain;

namespace Persistance;

public class LikePostConfigure : IEntityTypeConfiguration<LikePost>
{
    public void Configure(EntityTypeBuilder<LikePost> builder)
    {
        builder
         .HasOne(l => l.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.PostId)
         .OnDelete(DeleteBehavior.Restrict);
        

        builder.Property(l => l.IsActived).HasDefaultValue(true);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

    }


}
