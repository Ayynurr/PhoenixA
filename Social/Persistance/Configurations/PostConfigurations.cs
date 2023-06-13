using Persistance.Extension;

namespace Persistance.Configurations;

public class PostConfigurations : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ConfigureBaseEntity();
        builder
            .HasMany(p=>p.Likes)
            .WithOne(p=>p.Post)
            .HasForeignKey(p=>p.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
