using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.AuthValidator;

public class RegisterValidation : AbstractValidator<RegisterDto>
{
    public RegisterValidation()
    {
        RuleFor(u => u.Firstname)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(20);
        RuleFor(u => u.Lastname)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(20);
        RuleFor(u => u.Password)
            .NotEmpty()
            .NotNull()
            .Equal(u => u.ConfirmPassword);
        RuleFor(u => u.Username)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(15);
    }
}
