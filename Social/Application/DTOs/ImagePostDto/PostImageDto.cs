using Microsoft.AspNetCore.Http;

namespace Application.DTOs.ImagePostDto;

public class PostImageDto
{
    public IFormFileCollection Images { get; set; }
}
