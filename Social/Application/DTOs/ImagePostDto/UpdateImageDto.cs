using Microsoft.AspNetCore.Http;

namespace Application.DTOs.ImagePostDto;

public class UpdateImageDto
{
    public IFormFileCollection Images { get; set; }
}
