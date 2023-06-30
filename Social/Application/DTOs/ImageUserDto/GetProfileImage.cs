using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class GetProfileImage
{
    public int UserId { get; set; }
    public string ImageUrl { get; set; }    
    public string Image { get; set; }
}
