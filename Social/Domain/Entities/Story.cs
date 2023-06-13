using Domain.Entities.Base;

namespace Domain.Entities;

public class Story : BaseAuditable
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageName { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}
