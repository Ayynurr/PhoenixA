using Domain.Entities.Base;
using Domain.Entities.Entities;

namespace Persistance.Extension;

public static class EntityTypeExtentions
{
    public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        builder.Property(c=>c.IsActived).HasDefaultValue(true);
        builder.HasQueryFilter(c => c.IsDeleted == false);
    }
    public static void ConfigureAuditable<TEntity>(this EntityTypeBuilder<TEntity> builder)
      where TEntity : BaseAuditable
    {
        builder.Property(c => c.UpdatedDate).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
    }
}
