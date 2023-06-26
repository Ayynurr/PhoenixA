using Application.DTOs.CommentDto.AuthDto;
using FluentValidation;

namespace Application.Validations.AuthValidator;

public class LoginValidaton : AbstractValidator<LoginDto>
{
    public LoginValidaton()
    {
        RuleFor(u => u.Username).NotEmpty().NotNull();
        RuleFor(u => u.Password).NotEmpty().NotNull();
    }
}
