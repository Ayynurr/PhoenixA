using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class PostAllDto
{
    public string Content { get; set; } = null!;
    public List<IFormFile> Images { get; set; }
}
