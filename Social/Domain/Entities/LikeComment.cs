using Domain.Entities;
using Domain.Entities.Base;

namespace Domain;

public class LikeComment : BaseAuditable
{
    public int UserId { get; set; }
    public AppUser User { get; set; }
    public int CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
}
