using Application.DTOs;
using FluentValidation;

namespace Application.Validations;

public class CommentCreateValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateValidator()
    {
        RuleFor(p => p.Content)
            .NotEmpty()
            .NotNull();

    }
}
