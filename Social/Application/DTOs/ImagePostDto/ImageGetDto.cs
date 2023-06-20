namespace Application.DTOs.ImagePostDto;

public class ImageGetDto
{
    public int PostId { get; set; }
    public string ImageName { get; set; }
    public string Url { get; set; } = null!;
}
