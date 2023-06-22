using Domain.Entities.Base;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class UserImage:BaseAuditable
{
    public string ProfileImageName { get; set; }
    public string BackraundImageName { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string PathProfile { get; set; } = null!;
    public string PathBack { get; set; } = null!;
   
}
