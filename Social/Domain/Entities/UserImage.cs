using Domain.Entities.Base;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class UserImage:BaseAuditable
{
   
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string Path { get; set; }
    public bool IsBack { get; set; }
    public bool IsProfile { get; set; }
  

}
