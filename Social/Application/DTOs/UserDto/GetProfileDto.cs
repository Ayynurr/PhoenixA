using Domain.Entities;

namespace Application.DTOs;

public class GetProfileDto
{
    public string Username { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }


}
