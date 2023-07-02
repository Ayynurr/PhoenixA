namespace Application.DTOs;

public class UserAdminDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? BlockEndDate { get; set; }

}
