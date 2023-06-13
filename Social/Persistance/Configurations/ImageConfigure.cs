using Persistance.Extension;

namespace Persistance.Configurations;

public class ImageConfigure : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ConfigureBaseEntity();
        builder
            .HasOne(p=>p.Post)
            .WithMany(p=>p.Images)
            .HasForeignKey(p=>p.PostId)
            .OnDelete(DeleteBehavior.Cascade);



    }
}
