using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class AddPostDto
{

    public int GroupId { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
}
