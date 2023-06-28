namespace Domain.Entities;

public class GroupMembership
{
    public int UserId { get; set; }
    public AppUser User { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public Status Status { get; set; }
}
