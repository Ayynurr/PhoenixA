using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class UpdateProfileImage
{
    public IFormFile Images { get; set; }

}
