using Domain.Entities;
using Domain.Entities.Base;

namespace Domain;

public class LikePost : BaseAuditable
{
    public Post Post { get; set; } = null!;
    public int? PostId { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; }
}
