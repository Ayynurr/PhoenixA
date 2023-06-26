namespace Persistance.Configurations;

public class UserConfigure: IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);
        builder
                .HasMany(u => u.Friends)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
    }
}
