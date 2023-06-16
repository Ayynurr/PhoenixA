namespace Persistance.Configurations;

public class LikeConfigure : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder
         .HasOne(l => l.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.PostId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
