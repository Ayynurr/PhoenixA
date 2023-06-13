
using Persistance.Extension;

namespace Persistance.Configurations;

internal class CommentConfigurations : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ConfigureBaseEntity();
        builder.HasOne(c => c.TopComment)
            .WithMany(c => c.ReplyComments)
            .HasForeignKey(c => c.TopCommentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
