using System.Reflection;

namespace Persistance.DataContext;

public class AppDbContext : IdentityDbContext<AppUser,Role,int>
{
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    //public DbSet<Comment> Comments { get; set; }
    //public DbSet<Story> Stories { get; set; }
    //public DbSet<Image> Images { get; set; }
    //public DbSet<UserImage> UserImages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Like>()
        .HasOne(l => l.Post)
        .WithMany(p => p.Likes)
        .HasForeignKey(l => l.PostId)
        .OnDelete(DeleteBehavior.Cascade);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
