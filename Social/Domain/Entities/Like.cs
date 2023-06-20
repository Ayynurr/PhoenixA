using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Like : BaseAuditable
{
    public Post Post { get; set; } = null!;
    public int PostId { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public int CommentId { get; set; }
    public Comment Comment { get; set; }
}
