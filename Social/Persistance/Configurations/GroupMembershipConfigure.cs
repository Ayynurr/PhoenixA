using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configurations;

public class GroupMembershipConfigure : IEntityTypeConfiguration<GroupMembership>
{
    public void Configure(EntityTypeBuilder<GroupMembership> builder)
    {

        builder.HasKey(gm => new { gm.UserId, gm.GroupId });

        builder.HasOne(gm => gm.User)
            .WithMany(u => u.GroupMemberships)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gm => gm.Group)
            .WithMany(g => g.GroupMemberships)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(gm => gm.Status)
            .IsRequired()
            .HasConversion<string>();

    }
}


