using Application.DTOs.ImagePostDto;
using Domain.Entities;

namespace Application.DTOs;

public class PostGetDto 
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public List<ImageGetDto> Images { get; set; } = new List<ImageGetDto>();

}

