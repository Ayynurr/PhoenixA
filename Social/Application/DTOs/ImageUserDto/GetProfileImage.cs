using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class GetProfileImage
{
    public int UserId { get; set; }
    public string ProfileImage { get; set; }
    public string BackraoundImage { get; set; }
    public string UrlProfile { get; set; } = null!;
    public string UrlBackraound { get; set; } = null!;
}
