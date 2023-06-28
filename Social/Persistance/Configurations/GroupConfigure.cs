namespace Persistance.Configurations;

public class GroupConfigure : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);



        builder.HasMany(g => g.Posts)
            .WithOne(p => p.Group)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

      


        builder.Property(u => u.IsDeleted).HasDefaultValue(false);


    }
}
