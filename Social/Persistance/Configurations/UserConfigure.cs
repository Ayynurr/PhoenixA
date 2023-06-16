namespace Persistance.Configurations;

public class UserConfigure: IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);
       
    }
}
