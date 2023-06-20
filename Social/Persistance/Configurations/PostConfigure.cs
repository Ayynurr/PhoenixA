﻿using Persistance.Extension;

namespace Persistance.Configurations;

public class PostConfigure : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ConfigureBaseEntity();
        builder
            .HasMany(c => c.Likes)
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(l => l.IsActived).HasDefaultValue(true);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);
    }
}
