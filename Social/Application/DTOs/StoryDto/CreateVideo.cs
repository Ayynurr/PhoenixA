using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CreateVideo
{
    public string Content { get; set; } = null!;
    public IFormFile Videos { get; set; }
}