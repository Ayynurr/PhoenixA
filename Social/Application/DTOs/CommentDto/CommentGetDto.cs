namespace Application.DTOs;

public class CommentGetDto
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public int? ReplyCommentId { get; set; }
    public DateTime CreateDate { get; set; }
    public int PostId { get; set; }
    public ICollection<CommentGetDto> ReplyComment { get; set; }

}
