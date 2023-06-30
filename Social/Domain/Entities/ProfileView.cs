namespace Domain.Entities;

public class ProfileView
{
    public int Id { get; set; }
    public int ProfileOwnerId { get; set; } 
    public int VisitorId { get; set; } 
    public DateTime VisitDate { get; set; }
}
