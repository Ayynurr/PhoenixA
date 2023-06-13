namespace Application.DTOs;

public class CommentCreateDto
{
    public string Content { get; set; } = null!;
    public int UserId { get; set; }
}
