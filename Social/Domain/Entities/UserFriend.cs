namespace Domain.Entities;

public class UserFriend
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public int FriendId { get; set; }
    public AppUser Friend { get; set; } = null!;
    public FriendStatus Status { get; set; }

}
