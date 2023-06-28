using Domain.Entities.Base;
namespace Domain.Entities;

public class Story : BaseAuditable
{
    public string? Content { get; set; }
    public string? ImageName { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; }
    public string? VideoName { get; set; }
    public bool IsArchived { get; set; }

}
