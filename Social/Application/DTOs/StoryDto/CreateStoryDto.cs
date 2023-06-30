using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CreateStoryDto
{
    public string Content { get; set; } = null!;
    public IFormFile Image { get; set; }

}
