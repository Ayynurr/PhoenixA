using Domain.Entities.Base;

namespace Domain.Entities;

public class Post : BaseAuditable
{
    public Post()
    {
        Likes = new HashSet<Like>();
    }

    public string Content { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Like> Likes { get; set; }
    public ICollection<Image> Images { get; set; }

}
