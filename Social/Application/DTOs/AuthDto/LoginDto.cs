using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDto;

public class LoginDto
{
    public string Username { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
