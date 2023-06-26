using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Comment : BaseAuditable
{
   
    public string Content { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public int UserId { get; set; }
    public Comment? TopComment { get; set; } 
    public int? TopCommentId { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public ICollection<Comment> ReplyComments { get; set; }
    public ICollection<LikeComment> Likes { get; set; }
    

}
