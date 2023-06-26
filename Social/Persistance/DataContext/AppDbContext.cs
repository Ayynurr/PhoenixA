using Domain;
using System.Reflection;

namespace Persistance.DataContext;

public class AppDbContext : IdentityDbContext<AppUser,Role,int>
{
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<LikeComment> LikeComments { get; set; }
    public DbSet<LikePost> LikePosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<UserImage> UserImages { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
