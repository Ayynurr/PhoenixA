namespace Application.DTOs;

public class LikeGetDto
{
    public int Id { get; set; }
    public DateTime LikeDate { get; set; }
    public string UserId { get; set; }
    public int PostId { get; set; }
}
