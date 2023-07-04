using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDto;

public class RegisterDto
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime Birthday { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }


}
