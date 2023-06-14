using Domain.Entities.Base;
using Domain.Entities.Entities;

namespace Domain.Entities;

public class UserImage:BaseAuditable
{
    public string ImageName { get; set; }
    public bool IsProfileImage { get; set; }
    public bool IsBacroundImage { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}
