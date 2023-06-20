using Domain.Entities;

namespace Application.DTOs;

public class CommentCreateDto
{
    public string Content { get; set; }
    public int PostId { get; set; }
    public int? ReplyCommentId { get; set; }
}
