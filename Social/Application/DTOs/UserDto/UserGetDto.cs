using Domain.Entities;

namespace Application.DTOs;

public class UserGetDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime BirthDate { get; set; } //backround service 
    public string? Bio { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }
}
