namespace Application.DTOs;

public class LikeCreateDto
{
    public int CommentId { get; set; } 
    public int UserId { get; set; }
    public int PostId { get; set; }
}
