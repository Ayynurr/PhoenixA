using Domain.Entities.Base;

namespace Domain.Entities;

public class Like : BaseAuditable
{
   
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
