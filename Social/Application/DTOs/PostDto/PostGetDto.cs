using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class PostGetDto 
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
}
