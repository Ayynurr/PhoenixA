namespace Persistance.Configurations;

public class UserFriendConfigure : IEntityTypeConfiguration<UserFriend>
{
    public void Configure(EntityTypeBuilder<UserFriend> builder)
    {
        builder.ToTable("UserFriends");

        // Primary key
        builder.HasKey(uf => uf.Id);

        // Relationships
        builder.HasOne(uf => uf.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        //builder.HasOne(uf => uf.Friend)
        //    .WithMany(u => u.Friends)
        //    .HasForeignKey(uf => uf.FriendId)
        //    .OnDelete(DeleteBehavior.Cascade); // Dış anahtar kısıtlaması için silme davranışını belirtin (NO ACTION, SET NULL, SET DEFAULT)


    }
}
