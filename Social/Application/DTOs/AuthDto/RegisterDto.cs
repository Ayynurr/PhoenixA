
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class RegisterDto
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime Birthday { get; set; } 
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; } 


}
