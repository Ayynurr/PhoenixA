using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDto;

public class ResetPasswordDto
{
    //public string NewPassword { get; set; }
    //public string ConfirmPassword { get; set; }
    //public string Email { get; set; }
    //public string Token { get; set; }
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    [DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; }

}
