using Domain.Entities;

namespace Application.DTOs;

public class UserUpdateDto
{
    public DateTime BirthDate { get; set; } //backround service 
    public string? Bio { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }
   
  

}
