using Microsoft.EntityFrameworkCore;

namespace Persistance.Configurations;

public class UserFriendConfigure : IEntityTypeConfiguration<UserFriend>
{
    public void Configure(EntityTypeBuilder<UserFriend> builder)
    {
        builder.ToTable("UserFriends");

        builder.HasKey(uf => uf.Id);

        builder.HasOne(uf => uf.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        
    }
}
