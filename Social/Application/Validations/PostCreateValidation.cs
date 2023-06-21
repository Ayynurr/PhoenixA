using Application.DTOs;
using Domain.Entities;
using FluentValidation;

namespace Application.Validations;

public class PostCreateValidation : AbstractValidator<PostCreateDto>
{
    public PostCreateValidation()
    {
        RuleFor(p => p.Content)
            .NotEmpty()
            .NotNull();
    }
}
