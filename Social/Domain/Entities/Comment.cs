using Domain.Entities.Base;

namespace Domain.Entities;

public class Comment : BaseAuditable
{
    public Comment()
    {
        ReplyComments = new HashSet<Comment>();
    }
    public string Content { get; set; } = null!;
    public User User { get; set; } = null!;
    public int UserId { get; set; } 
    public int TopCommentId { get; set; }
    public Comment TopComment { get; set; } = null!;
    public ICollection<Comment> ReplyComments { get; set; }


}
