namespace Application.DTOs;

public class UserFriendDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserGetDto Friend { get; set; }
}
