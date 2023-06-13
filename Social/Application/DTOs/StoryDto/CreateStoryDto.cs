namespace Application.DTOs;

public class CreateStoryDto
{
    public string Content { get; set; } = null!;
    public int UserId { get; set; }
}
