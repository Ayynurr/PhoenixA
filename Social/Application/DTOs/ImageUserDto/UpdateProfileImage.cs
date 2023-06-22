using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class UpdateProfileImage
{
    public IFormFile ProfileImage { get; set; }
    public IFormFile BackImage { get; set; }

}
