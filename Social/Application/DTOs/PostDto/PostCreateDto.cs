using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public  class PostCreateDto
{
    public string Content { get; set; } = null!;
    public List<IFormFile> Images { get; set; }

}

