﻿using Persistance.Extension;

namespace Persistance.Configurations;

public class CommentConfigure : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ConfigureBaseEntity();

        builder
          .HasOne(c => c.Post)
           .WithMany(c => c.Comments)
           .HasForeignKey(c => c.PostId)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.TopComment)
            .WithMany(c => c.ReplyComments)
            .HasForeignKey(c => c.TopCommentId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
